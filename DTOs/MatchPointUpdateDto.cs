namespace PoolBrackets_backend_dotnet.DTOs
{
    public class MatchPointUpdateDto
    {
        public required int Id { get; set; }
        public required int FirstPlayerPoint { get; set; }
        public required int SecondPlayerPoint { get; set; }
    }
}
