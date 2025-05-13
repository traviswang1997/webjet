using System.ComponentModel.DataAnnotations;

namespace MovieCompare.Backend.Models
{
    /// <summary>
    /// Dto for the movie from external api endpoint
    /// </summary>
    
    public class MovieListDto 
    {
        public List<MovieDto> Movies { get; set; } = new();
    }
    
    public class MovieDto
    {
        [Required]
        public required string Id { get; set; }
        [Required]
        public required string Title { get; set; }
        public int? Year { get; set; }
        public string? Type { get; set; }
        [Required]
        public string? Poster { get; set; }
    }
}