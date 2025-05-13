using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using MovieCompare.Backend.Models;

namespace MovieCompare.Backend.Services
{
    public class ExternalMovieService : IExternalMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalMovieService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Get the list of movies from external API
        /// </summary>
        /// <param name="providerId"></param>
        public async Task<List<MovieDto>> GetMovies(string providerId)
        {
            var client = _httpClientFactory.CreateClient("cinemaExternalApi");
            string endpoint = providerId switch
            {
                "cinemaworld" => "/api/cinemaworld/movies",
                "filmworld"   => "/api/filmworld/movies",
                _ => throw new ArgumentException($"Unknown provider: {providerId}")
            };

            var movies = await client.GetFromJsonAsync<MovieListDto>(endpoint);
            return movies?.Movies ?? new List<MovieDto>();
        }

        /// <summary>
        /// Get detailed movie information (including price)
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="movieId"></param>
        public async Task<MovieDetailsDto?> GetMovieDetails(string providerId, string movieId)
        {
            var client = _httpClientFactory.CreateClient("cinemaExternalApi");
            string endpoint = providerId switch
            {
                "cinemaworld" => $"/api/cinemaworld/movie/{movieId}",
                "filmworld"   => $"/api/filmworld/movie/{movieId}",
                _ => throw new ArgumentException($"Unknown provider: {providerId}")
            };

            return await client.GetFromJsonAsync<MovieDetailsDto>(endpoint);
        }
    }
}