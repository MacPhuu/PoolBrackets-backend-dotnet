using System.Text.Json.Serialization;

namespace PoolBrackets_backend_dotnet.DTOs
{
    public class AddEventDto
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("venue")]
        public required string Venue { get; set; }

        [JsonPropertyName("location")]
        public required string Location { get; set; }

        [JsonPropertyName("date")]
        public required DateTime Date { get; set; }

        [JsonPropertyName("number_of_players")]
        public int? NumberOfPlayers { get; set; }
    }
}