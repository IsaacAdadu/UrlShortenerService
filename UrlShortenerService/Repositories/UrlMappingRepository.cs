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

        public async Task<UrlMapping> CreateMappingAsync(string originalUrl, string shortUrl, DateTime? expiresAt)
        {
            var mapping = new UrlMapping
            {
                OriginalUrl = originalUrl,
                ShortUrl = shortUrl,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };
            _context.UrlMappings.Add(mapping);
            await _context.SaveChangesAsync();
            return mapping;
        }

        public async Task<UrlMapping?> GetMappingByShortUrlAsync(string shortUrl)
        {
            return await _context.UrlMappings
           .Where(m => m.ShortUrl == shortUrl && (m.ExpiresAt == null || m.ExpiresAt > DateTime.UtcNow))
            .FirstOrDefaultAsync();
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

