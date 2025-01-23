using Microsoft.AspNetCore.Mvc;
using UrlShortenerService.Repositories;

namespace UrlShortenerService.Controllers
{
    public class UrlShortenerController:ControllerBase
    {
        private readonly IUrlMappingRepository _repository;

        public UrlShortenerController(IUrlMappingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] string originalUrl)
        {
            var shortUrl = GenerateShortUrl();
            var mapping = await _repository.CreateMappingAsync(originalUrl, shortUrl);
            return Ok(mapping.ShortUrl);
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> GetOriginalUrl(string shortUrl)
        {
            var mapping = await _repository.GetMappingByShortUrlAsync(shortUrl);
            if (mapping == null) return NotFound();
            return Redirect(mapping.OriginalUrl);
        }

        private string GenerateShortUrl()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }
    }
}
