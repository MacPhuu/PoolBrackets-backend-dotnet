using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("events")]
    public class Event
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public required string Name { get; set; }
        [Column("venue")]
        public required string Venue { get; set; }
        [Column("location")]
        public required string Location { get; set; }
        [Column("date")]
        public required DateTime Date { get; set; }
        [Column("is_displayed")]
        public required bool IsDisplayed { get; set; }
        [Column("number_of_players")]
        public required int NumberOfPlayers { get; set; }
        [Column("is_happen")]
        public required bool IsHappen { get; set; }
        [Column("create_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("update_at")]
        public DateTime? UpdateAt { get; set; }
    }
}
