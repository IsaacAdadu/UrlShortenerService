using UrlShortener.API.Models.Dtos;

namespace UrlShortener.API.Service
{
    public interface IUrlShortenerService
    {
        Task<string> ShortenUrlAsync(string originalUrl, TimeSpan? expiration = null);
        Task<string?> GetOriginalUrlAsync(string shortUrl);
        Task<UrlStatsResponse?> GetUrlStatsAsync(string shortUrl);
    }
}
