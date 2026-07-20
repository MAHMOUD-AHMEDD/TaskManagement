using AutoMapper;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }
        public async Task<ProjectDto?> GetProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            return project == null ? null : _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, string ownerId)
        {
            var project = _mapper.Map<Domain.Entities.Project>(createProjectDto);
            project.OwnerId = ownerId;

            // 1. Fetch the owner to populate the navigation property
            var owner = await _unitOfWork.ProjectMembers.FindAsync(pm => pm.UserId == ownerId);

            if (owner != null)
            {
                // Assigning this prevents AutoMapper from failing with a NullReferenceException
                project.Owner = owner.User;
            }

            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
            {
                throw new NotFoundException($"Project with id {id} not found.");
            }
            _mapper.Map(updateProjectDto, project);
            _unitOfWork.Projects.Update(project);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
            {
                throw new NotFoundException($"Project with id {id} not found.");
            }
            _unitOfWork.Projects.Delete(project);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
