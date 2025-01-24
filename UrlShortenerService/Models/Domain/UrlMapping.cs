namespace UrlShortenerService.Models.Domain
{
    public class UrlMapping
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public int AccessCount { get; set; } = 0;
    }
}
