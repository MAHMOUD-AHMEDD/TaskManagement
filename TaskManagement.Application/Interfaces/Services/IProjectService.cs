using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {

        Task<ICollection<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(string name, string description, string ownerId);
        Task UpdateProjectAsync(int id, string name, string description);
        Task DeleteProjectAsync(int id);



    }
}
