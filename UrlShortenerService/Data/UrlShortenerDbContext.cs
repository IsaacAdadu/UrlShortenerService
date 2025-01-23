using Microsoft.EntityFrameworkCore;
using UrlShortenerService.Models.Domain;

namespace UrlShortenerService.Data
{
    public class UrlShortenerDbContext:DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options) { }
        public DbSet<UrlMapping> UrlMappings { get; set; }
    }
}
