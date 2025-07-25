using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(CreateUserRequest request);
        Task<bool> UpdateUserAsync(int id, CreateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
    }
}