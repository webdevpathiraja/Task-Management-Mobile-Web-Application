using TaskManager.API.Models;

namespace TaskManager.API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<int> CreateTaskAsync(CreateTaskRequest request);
        Task<bool> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task<bool> UpdateTaskStatusAsync(int id, string status);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
    }
}