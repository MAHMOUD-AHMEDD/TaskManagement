using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Project;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectDto>> GetAllProjectsAsync(PaginationParams paginationParams);
        Task<ProjectDto?> GetProjectByIdAsync(int id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, string ownerId);
        Task UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto);
        Task DeleteProjectAsync(int id);
    }
}
