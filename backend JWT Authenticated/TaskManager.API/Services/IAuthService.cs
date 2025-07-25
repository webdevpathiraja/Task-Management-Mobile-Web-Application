using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        string GenerateJwtToken(User user);
    }
}