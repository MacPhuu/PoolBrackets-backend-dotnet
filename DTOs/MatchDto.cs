using PoolBrackets_backend_dotnet.Models.Enums;

namespace PoolBrackets_backend_dotnet.DTOs
{
    public class MatchDto
    {
        public int Id { get; set; }
        public required string EventName { get; set; }
        public required string Table { get; set; }
        public required string FirstPlayerName { get; set; }
        public required int FirstPlayerPoint { get; set; }
        public string? SecondPlayerName { get; set; }
        public required int SecondPlayerPoint { get; set; }
        public required bool IsStart { get; set; }
        public required bool IsFinish { get; set; }
        public MatchStageEnum? Stage { get; set; }
    }
}
