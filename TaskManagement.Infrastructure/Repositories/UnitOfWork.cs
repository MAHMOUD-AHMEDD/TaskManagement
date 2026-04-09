using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IGenericRepository<Project>? _projects;
    private IGenericRepository<Domain.Entities.Task>? _tasks;
    private IGenericRepository<ProjectMember>? _projectMembers;
    private IGenericRepository<TaskAssignment>? _taskAssignments;
    private IGenericRepository<Comment>? _comments;
    private IGenericRepository<Label>? _labels;
    private IGenericRepository<TaskLabel>? _taskLabels;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Project> Projects =>
        _projects ??= new GenericRepository<Project>(_context);

    public IGenericRepository<Domain.Entities.Task> Tasks =>
        _tasks ??= new GenericRepository<Domain.Entities.Task>(_context);

    public IGenericRepository<ProjectMember> ProjectMembers =>
        _projectMembers ??= new GenericRepository<ProjectMember>(_context);

    public IGenericRepository<TaskAssignment> TaskAssignments =>
        _taskAssignments ??= new GenericRepository<TaskAssignment>(_context);

    public IGenericRepository<Comment> Comments =>
        _comments ??= new GenericRepository<Comment>(_context);

    public IGenericRepository<Label> Labels =>
        _labels ??= new GenericRepository<Label>(_context);

    public IGenericRepository<TaskLabel> TaskLabels =>
        _taskLabels ??= new GenericRepository<TaskLabel>(_context);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}