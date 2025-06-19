using PoolBrackets_backend_dotnet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoolBrackets_backend_dotnet.DTOs
{
    public class PlayerHistoryDto
    { 
        public required int PlayerId { get; set; }
        public required int EventId { get; set; }
        public int? GroupStageTotal { get; set; }
        public int? GroupStageWin { get; set; }
        public int? KnockoutStageTotal { get; set; }
        public int? KnockoutStageWin { get; set; }
    }
}
