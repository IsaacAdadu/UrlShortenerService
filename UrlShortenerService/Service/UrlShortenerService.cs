using Microsoft.AspNetCore.DataProtection.Repositories;
using UrlShortener.API.Models.Dtos;
using UrlShortenerService.Repositories;

namespace UrlShortener.API.Service
{
    public class UrlShortenerService:IUrlShortenerService
    {
        private readonly IUrlMappingRepository _repository;

        public UrlShortenerService(IUrlMappingRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> ShortenUrlAsync(string originalUrl)
        {
            // Validate URL
            if (!IsValidUrl(originalUrl))
                throw new ArgumentException("Invalid URL");

            // Check if URL already exists
            var existingMapping = await _repository.GetByOriginalUrlAsync(originalUrl);
            if (existingMapping != null)
                return existingMapping.ShortUrl;

            // Generate unique short URL
            string shortUrl;
            do
            {
                shortUrl = GenerateShortUrl();
            } while (await _repository.GetMappingByShortUrlAsync(shortUrl) != null);

            // Create and save mapping
            await _repository.CreateMappingAsync(originalUrl, shortUrl);
            return shortUrl;
        }

        public async Task<string?> GetOriginalUrlAsync(string shortUrl)
        {
            var mapping = await _repository.GetMappingByShortUrlAsync(shortUrl);
            if (mapping == null)
                return null;

            // Increment access count
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
            // Generate a 7-character unique short URL
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
