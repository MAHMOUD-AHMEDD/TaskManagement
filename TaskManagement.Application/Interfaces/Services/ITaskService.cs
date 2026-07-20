using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId);
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
        Task UpdateTaskAsync(int id, UpdateTaskDto dto);
        Task DeleteTaskAsync(int id);
        Task AssignLabelToTaskAsync(int taskId, int labelId);
        Task AssignTaskAsync(string userId, int taskId);
    }
}
