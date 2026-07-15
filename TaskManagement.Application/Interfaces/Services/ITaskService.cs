using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Domain.Entities.Task>> GetAllTasksAsync();
        Task<Domain.Entities.Task> GetTaskByIdAsync(int id);
        System.Threading.Tasks.Task CreateTaskAsync(int projectId, string title, string description);
        System.Threading.Tasks.Task UpdateTaskAsync(int projectId, string title, string description, Domain.Enums.TaskStatus status, TaskPriority priority);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
        System.Threading.Tasks.Task CreateTaskLabelAsync(int taskId, int labelId);
        System.Threading.Tasks.Task AssignTaskUser(string userId, int taskId);



    }
}
