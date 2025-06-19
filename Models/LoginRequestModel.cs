namespace PoolBrackets_backend_dotnet.Models
{
    public class LoginRequestModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}