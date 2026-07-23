using AutoMapper;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskEntity = TaskManagement.Domain.Entities.Task;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<TaskDto>> GetAllTasksAsync(PaginationParams paginationParams)
        {
            var (tasks, totalCount) = await _unitOfWork.Tasks.GetPagedAsync(paginationParams.PageNumber, paginationParams.PageSize);
            var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);

            return new PagedResult<TaskDto>
            {
                Items = taskDtos,
                TotalCount = totalCount,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
        }

        public async Task<PagedResult<TaskDto>> GetTasksByProjectIdAsync(int projectId, PaginationParams paginationParams)
        {
            var (tasks, totalCount) = await _unitOfWork.Tasks.GetPagedAsync(
                t => t.ProjectId == projectId,
                paginationParams.PageNumber,
                paginationParams.PageSize
            );
            var taskDtos = _mapper.Map<IEnumerable<TaskDto>>(tasks);

            return new PagedResult<TaskDto>
            {
                Items = taskDtos,
                TotalCount = totalCount,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            return task == null ? null : _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            var task = _mapper.Map<TaskEntity>(dto);
            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TaskDto>(task);
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(int id, UpdateTaskDto dto)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
            {
                throw new NotFoundException($"Task with id {id} not found.");
            }
            _mapper.Map(dto, task);
            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);
            if (task == null)
            {
                throw new NotFoundException($"Task with id {id} not found.");
            }
            _unitOfWork.Tasks.Delete(task);
            await _unitOfWork.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AssignLabelToTaskAsync(int taskId, int labelId)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new NotFoundException($"Task with id {taskId} not found.");
            }
            var label = await _unitOfWork.Labels.GetByIdAsync(labelId);
            if (label == null)
            {
                throw new NotFoundException($"Label with id {labelId} not found.");
            }

            if (label.ProjectId != task.ProjectId)
                throw new BadRequestException("Label does not belong to the same project as the task.");




            task.TaskLabels.Add(new TaskLabel
            {
                TaskId = taskId,
                LabelId = labelId
            });
            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }


        public async System.Threading.Tasks.Task AssignTaskAsync(string userId, int taskId)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
            if (task == null)
            {
                throw new NotFoundException($"Task with id {taskId} not found.");
            }
            // Check the user is actually a member of this task's project
            var isMember = await _unitOfWork.ProjectMembers.FindAsync(
                m => m.UserId == userId && m.ProjectId == task.ProjectId);
            if (isMember == null)
            {
                throw new BadRequestException("User is not a member of this task's project.");
            }
            task.TaskAssignments.Add(new TaskAssignment
            {
                TaskId = taskId,
                UserId = userId
            });
            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }



    }
}
