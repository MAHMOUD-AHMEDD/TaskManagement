namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Domain.Entities.Task>> GetAllTasksAsync();
        Task<Domain.Entities.Task> GetTaskByIdAsync(int id);
        System.Threading.Tasks.Task CreateTaskAsync(int projectId, string title, string description);
        System.Threading.Tasks.Task UpdateTaskAsync(int projectId, string title, string description);
        System.Threading.Tasks.Task DeleteTaskAsync(int id);
        System.Threading.Tasks.Task CreateTaskLabelAsync(int projectId, int taskId, string name);
        System.Threading.Tasks.Task AssignTaskUser(int userId, int taskId);



    }
}
