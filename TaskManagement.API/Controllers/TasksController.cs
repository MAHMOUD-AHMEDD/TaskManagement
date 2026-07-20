using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            var task = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            await _taskService.UpdateTaskAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpPost("{taskId}/labels/{labelId}")]
        public async Task<IActionResult> AssignLabelToTask(int TaskId, int LabelId)
        {
            await _taskService.AssignLabelToTaskAsync(TaskId, LabelId);
            return NoContent();
        }

        [HttpPost("{taskId}/assign/{userId}")]
        public async Task<IActionResult> AssignUserToTask(int taskId, string userId)
        {
            await _taskService.AssignTaskAsync(userId, taskId);
            return NoContent();
        }
    }
}
