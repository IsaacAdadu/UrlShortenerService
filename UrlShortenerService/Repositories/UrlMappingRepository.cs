using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Data;
using UrlShortenerService.Models.Domain;

namespace UrlShortenerService.Repositories
{
    public class UrlMappingRepository:IUrlMappingRepository
    {
        private readonly UrlShortenerDbContext _context;

        public UrlMappingRepository(UrlShortenerDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMapping> CreateMappingAsync(string originalUrl, string shortUrl)
        {
            var mapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl
            };
            _context.UrlMappings.Add(mapping);
            await _context.SaveChangesAsync();
            return mapping;
        }

        public async Task<UrlMapping?> GetMappingByShortUrlAsync(string shortUrl)
        {
            return await _context.UrlMappings
                .FirstOrDefaultAsync(m => m.ShortUrl == shortUrl);
        }

        public async Task<UrlMapping?> GetByOriginalUrlAsync(string originalUrl)
        {
            return await _context.UrlMappings
                .FirstOrDefaultAsync(m => m.OriginalUrl == originalUrl);
        }

        public async Task IncrementAccessCountAsync(string shortUrl)
        {
            var mapping = await GetMappingByShortUrlAsync(shortUrl);
            if (mapping != null)
            {
                mapping.AccessCount++;
                await _context.SaveChangesAsync();
            }
        }
    }
}

