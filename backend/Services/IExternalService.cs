namespace MovieCompare.Backend.Services
{
    using MovieCompare.Backend.Models;
    
    /// <summary>
    /// interface: get movie from external api
    /// </summary>
    public interface IExternalMovieService
    {
        /// <summary>
        /// get movie by provider name
        /// </summary>
        /// <param name="providerId">provider name</param>
        /// <returns></returns>
        Task<List<MovieDto>> GetMovies(string providerId);

        /// <summary>
        /// get movie details from external api by using provider name and movie id
        /// </summary>
        /// <param name="providerId">provider name</param>
        /// <param name="movieId">movie id</param>
        Task<MovieDetailsDto?> GetMovieDetails(string providerId, string movieId);
    }
}