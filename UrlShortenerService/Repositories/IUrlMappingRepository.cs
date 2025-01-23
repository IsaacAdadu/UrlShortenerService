﻿using UrlShortenerService.Models.Domain;

namespace UrlShortenerService.Repositories
{
    public interface IUrlMappingRepository
    {
        Task<UrlMapping> CreateMappingAsync(string originalUrl, string shortUrl);
        Task<UrlMapping?> GetMappingByShortUrlAsync(string shortUrl);
        Task<UrlMapping?> GetByOriginalUrlAsync(string originalUrl);
        Task IncrementAccessCountAsync(string shortUrl);
    }
}
