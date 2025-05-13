using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MovieCompare.Backend.Models;
using MovieCompare.Backend.Services;

namespace MovieCompare.Backend.Services
{
    /// <summary>
    /// Service that fetches integrated movie summaries and full details/prices for BFF.
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly IExternalMovieService _external;
        private readonly IMemoryCache _cache;
        private static readonly MemoryCacheEntryOptions CacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        public MovieService(IExternalMovieService external, IMemoryCache cache)
        {
            _external = external;
            _cache = cache;
        }

        public async Task<List<MovieSummaryDto>> GetMovieSummaries(string providerId, bool forceRefresh)
        {
            var cacheKey = $"bff:movie_summary:{providerId}";
            if (!forceRefresh && _cache.TryGetValue(cacheKey, out List<MovieSummaryDto> cached))
            {
                return cached;
            }

            // Fetch list and populate initial price/provider
            var movieList = await _external.GetMovies(providerId);
            var summaries = new List<MovieSummaryDto>();
            foreach (var movie in movieList)
            {
                var summary = new MovieSummaryDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    Type = movie.Type,
                    Poster = movie.Poster,
                    ProviderId = providerId
                };
                summaries.Add(summary);
            }

            _cache.Set(cacheKey, summaries, CacheOptions);
            return summaries;
        }

        public async Task<MovieDetailsDto> GetMovieDetails(string providerId, string movieId)
        {
            var cacheKey = $"bff:movie_details:{providerId}-{movieId}";
            if (_cache.TryGetValue(cacheKey, out MovieDetailsDto cached))
            {
                return cached;
            }

            // Fetch list and populate initial price/provider
            var movie = await _external.GetMovieDetails(providerId, movieId);
            if (movie != null)
            {
                _cache.Set(cacheKey, movie, CacheOptions);
            }
            return movie;
        }


        /// <summary>
        /// @TODO: stream server push for update movies..
        /// </summary>
        /// <param name="primaryProvider"></param>
        /// <param name="fallbackProvider"></param>
        /// <param name="forceRefresh"></param>
        /// <returns></returns>
        public async Task<List<MovieSummaryDto>> StreamIntegratedMovieSummary(
        string primaryProvider,
        string fallbackProvider,
        bool forceRefresh = false)
        {
            // 1. Load primary summaries (with initial price)
            var primaryList = await GetMovieSummaries(primaryProvider, forceRefresh);

            // 2. Fetch fallback list
            List<MovieDto> fallbackList;
            try
            {
                fallbackList = await _external.GetMovies(fallbackProvider);
            }
            catch
            {
                // If fallback fails, return primary-only data
                return primaryList;
            }
            var fallbackMap = fallbackList.ToDictionary(m => m.Title, StringComparer.OrdinalIgnoreCase);

            // 3. Compare and update
            foreach (var summary in primaryList)
            {
                if (fallbackMap.TryGetValue(summary.Title, out var dto))
                {
                    // Add fallback to availability
                    if (!summary.AvailableIn.Contains(fallbackProvider))
                        summary.AvailableIn.Add(fallbackProvider);

                    var fallbackDetail = await _external.GetMovieDetails(fallbackProvider, dto.Id);
                    if (fallbackDetail != null && fallbackDetail.Price < (summary.Price ?? decimal.MaxValue))
                    {
                        summary.Price = fallbackDetail.Price;
                        summary.ProviderId = fallbackProvider;
                    }
                }
            }

            return primaryList;
        }
    }
}
