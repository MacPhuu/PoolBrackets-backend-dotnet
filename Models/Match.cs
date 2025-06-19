using PoolBrackets_backend_dotnet.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("matches")]
    public class Match
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("event_id")]
        public required int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }  

        [Column("table")]
        public string? Table { get; set; }

        [Column("first_player_id")]
        public required int FirstPlayerId { get; set; }

        [ForeignKey("FirstPlayerId")]
        public Player? FirstPlayer { get; set; } 
        [Column("first_player_point")]
        public required int FirstPlayerPoint { get; set; }

        [Column("second_player_id")]
        public int? SecondPlayerId { get; set; }

        [ForeignKey("SecondPlayerId")]
        public Player? SecondPlayer { get; set; } 

        [Column("second_player_point")]
        public required int SecondPlayerPoint { get; set; }

        [Column("is_start")]
        public required bool IsStart { get; set; }

        [Column("is_finish")]
        public required bool IsFinish { get; set; }
        [Column("stage")]
        public MatchStageEnum? Stage { get; set; }
        [Column("branch")]
        public MatchBranchEnum? Branch { get; set; }
    }
}
