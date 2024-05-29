using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogOfFootballPlayers_test1.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [JsonIgnore]
        public ICollection<Footballer>? Footballers { get; set; }
    }
}
