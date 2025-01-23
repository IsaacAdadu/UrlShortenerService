namespace UrlShortener.API.Models.Dtos
{
    public class UrlStatsResponse
    {
        public string ShortUrl { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public int AccessCount { get; set; }
    }
}
