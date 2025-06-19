using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<UserLoginDto> LogginAsync(LoginRequestModel loginInfo);
    }
}
