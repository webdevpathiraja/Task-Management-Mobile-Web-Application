using System.Data;
using System.Data.SqlClient;
using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly string _connectionString;
        private readonly IQueryService _queryService;

        public TaskService(IConfiguration configuration, IQueryService queryService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException("Connection string not found");
            _queryService = queryService;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = new List<TaskItem>();
            var query = _queryService.GetQuery("GetTasksByUserId");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                tasks.Add(MapTaskFromReader(reader));
            }

            return tasks;
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            var query = _queryService.GetQuery("GetTaskById");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                return MapTaskFromReader(reader);
            }

            return null;
        }

        public async Task<int> CreateTaskAsync(CreateTaskRequest request)
        {
            var query = _queryService.GetQuery("CreateTask");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", request.Title);
            command.Parameters.AddWithValue("@Description", request.Description ?? string.Empty);
            command.Parameters.AddWithValue("@Status", request.Status);
            command.Parameters.AddWithValue("@UserId", request.UserId);
            command.Parameters.AddWithValue("@DueDate", (object?)request.DueDate ?? DBNull.Value);
            
            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            var query = _queryService.GetQuery("UpdateTask");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Title", request.Title);
            command.Parameters.AddWithValue("@Description", request.Description ?? string.Empty);
            command.Parameters.AddWithValue("@Status", request.Status);
            command.Parameters.AddWithValue("@DueDate", (object?)request.DueDate ?? DBNull.Value);
            
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateTaskStatusAsync(int id, string status)
        {
            var query = _queryService.GetQuery("UpdateTaskStatus");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Status", status);
            
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var query = _queryService.GetQuery("DeleteTask");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            
            await connection.OpenAsync();
            var rowsAffected = await command.ExecuteNonQueryAsync();
            
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            var tasks = new List<TaskItem>();
            var query = _queryService.GetQuery("GetAllTasks");

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var task = MapTaskFromReader(reader);
                task.UserName = reader.GetString("UserName");
                tasks.Add(task);
            }

            return tasks;
        }

        private static TaskItem MapTaskFromReader(SqlDataReader reader)
        {
            return new TaskItem
            {
                Id = reader.GetInt32("Id"),
                Title = reader.GetString("Title"),
                Description = reader.GetString("Description"),
                Status = reader.GetString("Status"),
                UserId = reader.GetInt32("UserId"),
                CreatedDate = reader.GetDateTime("CreatedDate"),
                DueDate = reader.IsDBNull("DueDate") ? null : reader.GetDateTime("DueDate")
            };
        }
    }
}