using Microsoft.AspNetCore.Mvc;
using MovieCompare.Backend.Models;
using MovieCompare.Backend.Services;

namespace MovieCompare.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IExternalMovieService _external;
        private readonly string[] _providers = new[] { "cinemaworld", "filmworld" }; //hard code for now, scalable to dynamic cinemas

        public HealthController(IExternalMovieService external) => _external = external;

        [HttpGet("health")]
        public async Task<ActionResult<List<ProviderDto>>> GetProvidersHealthStatus()
        {
            var results = new List<ProviderDto>();

            foreach (var provider in _providers)
            {
                bool healthy;
                try
                {
                    await _external.GetMovies(provider);
                    healthy = true;
                }
                catch (Exception ex)
                {
                    healthy = false;
                }
                results.Add(new ProviderDto { ProviderId = provider, IsHealthy = healthy });
            }

            // Sort healthy one first
            var ordered = results.OrderByDescending(r => r.IsHealthy).ToList();
            return Ok(ordered);
        }
    }
}