namespace MovieCompare.Backend.Models
{
    /// <summary>
    /// Dto for the movie details from external api endpoint
    /// </summary>
    public class MovieDetailsDto : MovieDto
    {
        public string? Rated { get; set; }
        public string? Released { get; set; }
        public string? Runtime { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Writer { get; set; }
        public string? Actors { get; set; }
        public string? Plot { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
        public string? Awards { get; set; }
        public decimal? Metascore { get; set; }
        public decimal? Rating { get; set; }
        public string? Votes { get; set; }
        public decimal? Price { get; set; }
    }
}