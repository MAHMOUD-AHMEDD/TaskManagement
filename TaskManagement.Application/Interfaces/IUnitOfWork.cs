using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Project> Projects { get; }
    IGenericRepository<Domain.Entities.Task> Tasks { get; }
    IGenericRepository<ProjectMember> ProjectMembers { get; }
    IGenericRepository<TaskAssignment> TaskAssignments { get; }
    IGenericRepository<Comment> Comments { get; }
    IGenericRepository<Label> Labels { get; }
    IGenericRepository<TaskLabel> TaskLabels { get; }

    System.Threading.Tasks.Task<int> SaveChangesAsync();
}
