using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Models;
using TaskManager.API.Services;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, IUserService userService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByUserId(int userId)
        {
            try
            {
                // Verify user exists
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"User with ID {userId} not found");
                }

                var tasks = await _taskService.GetTasksByUserIdAsync(userId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks for user {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all tasks");
                return StatusCode(500, "An error occurred while retrieving tasks");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(CreateTaskRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest("Title is required");
                }

                if (request.UserId <= 0)
                {
                    return BadRequest("Valid UserId is required");
                }

                // Verify user exists
                var user = await _userService.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    return BadRequest($"User with ID {request.UserId} not found");
                }

                // Validate status
                var validStatuses = new[] { "Pending", "In Progress", "Completed" };
                if (!validStatuses.Contains(request.Status))
                {
                    return BadRequest("Status must be one of: Pending, In Progress, Completed");
                }

                var taskId = await _taskService.CreateTaskAsync(request);
                var createdTask = await _taskService.GetTaskByIdAsync(taskId);

                return CreatedAtAction(nameof(GetTaskById), new { id = taskId }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(500, "An error occurred while creating the task");
            }
        }

        [HttpGet("task/{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                
                if (task == null)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task {TaskId}", id);
                return StatusCode(500, "An error occurred while retrieving the task");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return BadRequest("Title is required");
                }

                // Validate status
                var validStatuses = new[] { "Pending", "In Progress", "Completed" };
                if (!validStatuses.Contains(request.Status))
                {
                    return BadRequest("Status must be one of: Pending, In Progress, Completed");
                }

                var success = await _taskService.UpdateTaskAsync(id, request);
                
                if (!success)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task {TaskId}", id);
                return StatusCode(500, "An error occurred while updating the task");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, UpdateTaskStatusRequest request)
        {
            try
            {
                // Validate status
                var validStatuses = new[] { "Pending", "In Progress", "Completed" };
                if (!validStatuses.Contains(request.Status))
                {
                    return BadRequest("Status must be one of: Pending, In Progress, Completed");
                }

                var success = await _taskService.UpdateTaskStatusAsync(id, request.Status);
                
                if (!success)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task status {TaskId}", id);
                return StatusCode(500, "An error occurred while updating the task status");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var success = await _taskService.DeleteTaskAsync(id);
                
                if (!success)
                {
                    return NotFound($"Task with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task {TaskId}", id);
                return StatusCode(500, "An error occurred while deleting the task");
            }
        }
    }
}