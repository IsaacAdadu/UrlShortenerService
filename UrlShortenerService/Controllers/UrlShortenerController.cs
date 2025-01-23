using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models.Dtos;
using UrlShortener.API.Service;
using UrlShortenerService.Repositories;

namespace UrlShortenerService.Controllers
{
    public class UrlShortenerController:ControllerBase
    {
        private readonly IUrlShortenerService _shortenerService;

        public UrlShortenerController(IUrlShortenerService shortenerService)
        {
            _shortenerService = shortenerService;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlShortenerRequest request)
        {
            try
            {
                var shortUrl = await _shortenerService.ShortenUrlAsync(request.OriginalUrl);
                return Ok(new { ShortUrl = shortUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> RedirectUrl(string shortUrl)
        {
            var originalUrl = await _shortenerService.GetOriginalUrlAsync(shortUrl);
            return originalUrl == null
                ? NotFound()
                : Redirect(originalUrl);
        }

        [HttpGet("stats/{shortUrl}")]
        public async Task<IActionResult> GetStats(string shortUrl)
        {
            var stats = await _shortenerService.GetUrlStatsAsync(shortUrl);
            return stats == null
                ? NotFound()
                : Ok(stats);
        }
    }
}
