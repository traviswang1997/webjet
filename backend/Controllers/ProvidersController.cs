using Microsoft.AspNetCore.Mvc;
using MovieCompare.Backend.Models;
using MovieCompare.Backend.Services;

namespace MovieCompare.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public ProvidersController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// pagination applied when sending response.
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="forceRefresh"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("{providerId}/movies")]
        public async Task<ActionResult<ProviderDto>> GetMoviesByProvider(
            [FromRoute] string providerId,
            [FromQuery] bool forceRefresh = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            bool healthy;
            var movies = new List<MovieSummaryDto>();
            var total = 0;
            try
            {
                var res = await _movieService.GetMovieSummaries(providerId, forceRefresh);
                total = res.Count;
                movies = res
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();
                healthy = true;
            }
            catch (Exception ex)
            {
                healthy = false;
            }
            var results = new
            {
                page,
                pageSize,
                total,
                ProviderDto = new ProviderDto { ProviderId = providerId, MovieSummaries = movies, IsHealthy = healthy }
            };

            return Ok(results);
        }
    }
}