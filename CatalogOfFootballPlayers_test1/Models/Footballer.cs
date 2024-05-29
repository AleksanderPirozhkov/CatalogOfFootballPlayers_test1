using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogOfFootballPlayers_test1.Models
{
    public class Footballer
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public Team? Team { get; set; }
        [Required]
        public string? Country { get; set; }
    }
}
