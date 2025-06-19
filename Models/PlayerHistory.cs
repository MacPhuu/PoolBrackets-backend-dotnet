using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.Models
{
    [Table("player_history")]
    public class PlayerHistory
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("player_id")]
        public required int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player? Player { get; set; }
        [Column("event_id")]
        public required int EventId { get; set; }
        [ForeignKey("EventId")]
        public Event? Event { get; set; }
        [Column("group_stage_total")]
        public int? GroupStageTotal { get; set; }
        [Column("group_stage_win")]
        public int? GroupStageWin {  get; set; }
        [Column("knockout_stage_total")]
        public int? KnockoutStageTotal   { get; set; }
        [Column("knockout_stage_win")]
        public int? KnockoutStageWin { get; set ; }
    }
}
