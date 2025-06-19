using PoolBrackets_backend_dotnet.Models.Enums;

namespace PoolBrackets_backend_dotnet.DTOs
{
    public class UserLoginDto
    {
        public required string UserName { get; set; }
        public required UserRoleEnum Role { get; set; }
        public required string Token { get; set; }
    }
}
