using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        private readonly User _allowedUser = new User
        {
            Id = 1,
            Name = "Aloka",
            Email = "aloka@example.com",
            CreatedDate = DateTime.UtcNow
        };

        public AuthService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // Check credentials
            if (request.Email != "aloka@example.com" || request.Password != "12345")
            {
                return Task.FromResult<LoginResponse?>(null);
            }

            var token = GenerateJwtToken(_allowedUser);

            var response = new LoginResponse
            {
                Token = token,
                User = _allowedUser
            };

            return Task.FromResult<LoginResponse?>(response);
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}