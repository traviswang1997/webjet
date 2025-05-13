namespace MovieCompare.Backend.Services
{
    using MovieCompare.Backend.Models;

    /// <summary>
    /// interface: movie price processing service
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// get movie summary from different provider/cinema
        /// </summary>
        /// <param name="providerId">cinema id</param>
        /// <param name="forceRefresh">true -> sending requst to hard refresh data</param>
        /// <returns></returns>
        Task<List<MovieSummaryDto>> GetMovieSummaries(string providerId, bool forceRefresh);

        /// <summary>
        /// get a single movie data from one provider/cinema
        /// </summary>
        /// <param name="providerId"></param>
        /// <param name="movieId"></param>
        /// <returns></returns>
        Task<MovieDetailsDto> GetMovieDetails(string providerId, string movieId);
    }
}