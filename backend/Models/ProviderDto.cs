namespace MovieCompare.Backend.Models
{
    /// <summary>
    /// provider/cinema id and all the movies.
    /// </summary>
    public class ProviderDto
    {
        public required string ProviderId { get; set; }
        public List<MovieSummaryDto> MovieSummaries { get; set;} = new ();
        public bool IsHealthy { get; set; }
    }
}