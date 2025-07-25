using System.Data;
using System.Data.SqlClient;
using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;
        private readonly IQueryService _queryService;

        public UserService(IConfiguration configuration, IQueryService queryService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string not found");
            _queryService = queryService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            var query = _queryService.GetQuery("GetAllUsers");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                });
            }

            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var query = _queryService.GetQuery("GetUserById");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Email = reader.GetString("Email"),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                };
            }

            return null;
        }

        public async Task<int> CreateUserAsync(CreateUserRequest request)
        {
            var query = _queryService.GetQuery("CreateUser");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", request.Name);
            command.Parameters.AddWithValue("@Email", request.Email);
            
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateUserAsync(int id, CreateUserRequest request)
        {
            var query = _queryService.GetQuery("UpdateUser");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", request.Name);
            command.Parameters.AddWithValue("@Email", request.Email);
            
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var query = _queryService.GetQuery("DeleteUser");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }
    }
}