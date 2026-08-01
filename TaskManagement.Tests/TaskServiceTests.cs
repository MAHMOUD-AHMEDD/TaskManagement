using AutoMapper;
using Moq;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskService _sut; // "sut" = System Under Test

        public TaskServiceTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _sut = new TaskService(_mockUnitOfWork.Object, _mockMapper.Object);

        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_WhenTaskExists_ReturnsTaskDto()
        {
            // Arrange
            var task = new Domain.Entities.Task { Id = 1, Title = "Test Task", ProjectId = 1 };
            var expectedDto = new Application.DTOs.Task.TaskDto { Id = 1, Title = "Test Task" };
            _mockUnitOfWork.Setup(u => u.Tasks.GetByIdAsync(1)).ReturnsAsync(task);
            _mockMapper.Setup(m => m.Map<Application.DTOs.Task.TaskDto>(task)).Returns(expectedDto);

            // Act
            var result = await _sut.GetTaskByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Title, result.Title);

        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskByIdAsync_WhenTaskDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Tasks.GetByIdAsync(1)).ReturnsAsync((Domain.Entities.Task?)null);
            // Act
            var result = await _sut.GetTaskByIdAsync(1);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_WhenTaskExists_DeletesTaskAndSavesChanges()
        {
            // Arrange
            var task = new Domain.Entities.Task { Id = 1, Title = "Test Task", ProjectId = 1 };
            _mockUnitOfWork.Setup(u => u.Tasks.GetByIdAsync(1)).ReturnsAsync(task);
            _mockUnitOfWork.Setup(u => u.Tasks.Delete(task));
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            // Act
            await _sut.DeleteTaskAsync(1);
            // Assert
            _mockUnitOfWork.Verify(u => u.Tasks.Delete(task), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_WhenTaskDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Tasks.GetByIdAsync(1)).ReturnsAsync((Domain.Entities.Task?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteTaskAsync(1));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_WhenTaskIsValid_CreatesTaskAndSavesChanges()
        {
            // Arrange
            var taskDto = new Application.DTOs.Task.CreateTaskDto { Title = "New Task", ProjectId = 1 };
            var task = new Domain.Entities.Task { Id = 1, Title = "New Task", ProjectId = 1 };
            _mockMapper.Setup(m => m.Map<Domain.Entities.Task>(taskDto)).Returns(task);
            _mockUnitOfWork.Setup(u => u.Tasks.AddAsync(task));
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            // Act
            await _sut.CreateTaskAsync(taskDto);
            // Assert
            _mockUnitOfWork.Verify(u => u.Tasks.AddAsync(task), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }



        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_WhenTaskExists_UpdatesTaskAndSavesChanges()
        {
            // Arrange
            var taskId = 1;
            var taskDto = new UpdateTaskDto { Title = "Updated Task", Priority = TaskPriority.High };
            var existingTask = new Domain.Entities.Task { Id = taskId, Title = "Old Task", ProjectId = 1 };

            _mockUnitOfWork.Setup(u => u.Tasks.GetByIdAsync(taskId)).ReturnsAsync(existingTask);
            _mockMapper.Setup(m => m.Map(taskDto, existingTask));
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _sut.UpdateTaskAsync(taskId, taskDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.Tasks.Update(existingTask), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskAsync_WhenTaskDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var taskId = 1;
            var taskDto = new UpdateTaskDto { Title = "Updated Task", Priority = TaskPriority.High };
            _mockUnitOfWork.Setup(u => u.Tasks.GetByIdAsync(taskId)).ReturnsAsync((Domain.Entities.Task?)null);
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateTaskAsync(taskId, taskDto));

        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasksByProjectIdAsync_WhenTasksExist_ReturnsTaskDtos()
        {
            // Arrange
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var projectId = 1;
            var tasks = new List<Domain.Entities.Task>
            {
                new Domain.Entities.Task { Id = 1, Title = "Task 1", ProjectId = projectId },
                new Domain.Entities.Task { Id = 2, Title = "Task 2", ProjectId = projectId }
            };
            var expectedDtos = tasks.Select(t => new TaskDto { Id = t.Id, Title = t.Title }).ToList();
            _mockUnitOfWork.Setup(u => u.Tasks.GetPagedAsync(
                    It.IsAny<Expression<Func<Domain.Entities.Task, bool>>>(),
                    paginationParams.PageNumber,
                    paginationParams.PageSize))
               .ReturnsAsync((tasks, 2));
            _mockMapper.Setup(m => m.Map<IEnumerable<TaskDto>>(tasks)).Returns(expectedDtos);
            // Act
            var result = await _sut.GetTasksByProjectIdAsync(projectId, paginationParams);
            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
        }


    }
}
