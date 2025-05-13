using System.ComponentModel.DataAnnotations;

namespace MovieCompare.Backend.Models
{
    /// <summary>
    /// Dto: movie detailed info
    /// </summary>
    public class MovieSummaryDto : MovieDto
    {
        [Required]
        public List<string> AvailableIn { get; set; } = new();
        public decimal? Price { get; set; }
        
        [Required]
        public required string? ProviderId {get; set;}
    }
}