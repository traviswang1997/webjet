using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MovieCompare.Backend.Models;
using MovieCompare.Backend.Services;

namespace MovieCompare.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// return movie's price from the given provider/cinema
        /// </summary>
        [HttpGet("{movieId}")]
        public async Task<ActionResult<MovieDetailsDto>> GetMovieDetailsById([FromQuery] string providerId, [FromRoute] string movieId)
        {
            var movie = await _movieService.GetMovieDetails(providerId, movieId);
            return Ok(movie);
        }

    }
}