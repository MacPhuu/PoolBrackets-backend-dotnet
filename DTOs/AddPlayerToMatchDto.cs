namespace PoolBrackets_backend_dotnet.DTOs
{
    public class AddPlayerToMatchDto
    {
        public required int EventId { get; set; }
        public required int PlayerId { get; set; }
    }
}
