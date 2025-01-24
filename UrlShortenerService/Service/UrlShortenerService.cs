using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.Caching.Memory;
using UrlShortener.API.Models.Dtos;
using UrlShortenerService.Repositories;

namespace UrlShortener.API.Service
{
    public class UrlShortenerService:IUrlShortenerService
    {
        private readonly IUrlMappingRepository _repository;
        private readonly IMemoryCache _cache;

        public UrlShortenerService(IUrlMappingRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<string> ShortenUrlAsync(string originalUrl, TimeSpan? expiration = null)
        {
            
            if (!IsValidUrl(originalUrl))
                throw new ArgumentException("Invalid URL");

           
            var existingMapping = await _repository.GetByOriginalUrlAsync(originalUrl);
            if (existingMapping != null)
                return existingMapping.ShortUrl;

            
            string shortUrl;
            do
            {
                shortUrl = GenerateShortUrl();
            } while (await _repository.GetMappingByShortUrlAsync(shortUrl) != null);

            // Determine expiration time (if any)
            DateTime? expiresAt = expiration.HasValue ? DateTime.UtcNow.Add(expiration.Value) : null;

            await _repository.CreateMappingAsync(originalUrl, shortUrl, expiresAt);
            _cache.Set(shortUrl, originalUrl, TimeSpan.FromMinutes(10));
            return shortUrl;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            // Check cache first
            if (_cache.TryGetValue(shortUrl, out string originalUrl))
                return originalUrl;


            var mapping = await _repository.GetMappingByShortUrlAsync(shortUrl);
            if (mapping == null)
                return null;

            // Cache the result
            _cache.Set(shortUrl, mapping.OriginalUrl, TimeSpan.FromMinutes(10));

            await _repository.IncrementAccessCountAsync(shortUrl);

            return mapping.OriginalUrl;
        }

        public async Task<UrlStatsResponse?> GetUrlStatsAsync(string shortUrl)
        {
            var mapping = await _repository.GetMappingByShortUrlAsync(shortUrl);
            return mapping != null
                ? new UrlStatsResponse
                {
                    ShortUrl = mapping.ShortUrl,
                    OriginalUrl = mapping.OriginalUrl,
                    AccessCount = mapping.AccessCount
                }
                : null;
        }

        private string GenerateShortUrl()
        {
            
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_")
                .Replace("+", "-")
                .Substring(0, 7);
        }

        private bool IsValidUrl(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }
            catch
            {
                return false;
            }
        }



        
    }
}
