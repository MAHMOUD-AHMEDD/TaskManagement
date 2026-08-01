using AutoMapper;
using Moq;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Project;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProjectService _sut; // "sut" = System Under Test

        public ProjectServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _sut = new ProjectService(_mockUnitOfWork.Object, _mockMapper.Object);
        }


        [Fact]
        public async System.Threading.Tasks.Task GetProjectByIdAsync_WhenProjectExists_ReturnsProjectDto()
        {
            // Arrange
            var project = new Project { Id = 1, Name = "Test Project", OwnerId = "user-1" };
            var expectedDto = new ProjectDto { Id = 1, Name = "Test Project" };

            _mockUnitOfWork.Setup(u => u.Projects.GetByIdAsync(1))
                            .ReturnsAsync(project);
            _mockMapper.Setup(m => m.Map<ProjectDto>(project))
                       .Returns(expectedDto);

            // Act
            var result = await _sut.GetProjectByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetProjectByIdAsync_WhenProjectDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Projects.GetByIdAsync(1))
                           .ReturnsAsync((Project?)null);

            // Act
            var result = await _sut.GetProjectByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteProjectAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Projects.GetByIdAsync(1))
                           .ReturnsAsync((Project?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteProjectAsync(1));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteProjectAsync_WhenProjectExists_DeletesProjectAndSavesChanges()
        {
            // Arrange
            var project = new Project { Id = 1, Name = "Test Project", OwnerId = "user-1" };
            _mockUnitOfWork.Setup(u => u.Projects.GetByIdAsync(1))
                           .ReturnsAsync(project);

            // Act
            await _sut.DeleteProjectAsync(1);

            // Assert
            _mockUnitOfWork.Verify(u => u.Projects.Delete(project), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateProjectAsync_WhenCalled_AddsProjectAndSavesChanges()
        {
            // Arrange
            var createDto = new CreateProjectDto { Name = "New Project", Description = "Test" };
            var mappedProject = new Project { Name = "New Project", Description = "Test" };
            var resultDto = new ProjectDto { Id = 1, Name = "New Project" };

            _mockMapper.Setup(m => m.Map<Project>(createDto)).Returns(mappedProject);
            _mockMapper.Setup(m => m.Map<ProjectDto>(mappedProject)).Returns(resultDto);

            //            required because CreateProjectAsync now looks up the owner's ProjectMember
            _mockUnitOfWork.Setup(u => u.ProjectMembers.FindAsync(It.IsAny<Expression<Func<ProjectMember, bool>>>()))
                   .ReturnsAsync((ProjectMember?)null);


            _mockUnitOfWork.Setup(u => u.Projects.AddAsync(mappedProject))
                  .Returns(System.Threading.Tasks.Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
                           .ReturnsAsync(1);

            // Act
            var result = await _sut.CreateProjectAsync(createDto, "owner-123");

            // Assert
            Assert.Equal("owner-123", mappedProject.OwnerId); // confirm OwnerId was set manually
            _mockUnitOfWork.Verify(u => u.Projects.AddAsync(mappedProject), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.Equal(resultDto.Name, result.Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsync_WhenProjectDoesNotExist_ThrowsNotFoundException()
        {

            // Arrange
            var updateDto = new UpdateProjectDto { Name = "Updated Project", Description = "Updated" };
            _mockUnitOfWork.Setup(u => u.Projects.GetByIdAsync(1))
                            .ReturnsAsync((Project?)null);
            var mappedProject = new Project
            {

                Name = "Updated Project",
                Description = "Updated"
            };
            _mockMapper.Setup(m => m.Map(updateDto, It.IsAny<Project>())).Returns(mappedProject);
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateProjectAsync(1, updateDto));

        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsync_WhenProjectExists_UpdatesProjectAndSavesChanges()
        {
            // Arrange
            var existingProject = new Project { Id = 1, Name = "Old Project", Description = "Old", OwnerId = "owner-123" };
            var updateDto = new UpdateProjectDto { Name = "Updated Project", Description = "Updated" };

            _mockUnitOfWork.Setup(u => u.Projects.GetByIdAsync(1))
                   .ReturnsAsync(existingProject);

            _mockMapper
                .Setup(m => m.Map(updateDto, existingProject))
                .Callback(() =>
                {
                    existingProject.Name = updateDto.Name;
                    existingProject.Description = updateDto.Description;
                });

            // Act
            await _sut.UpdateProjectAsync(1, updateDto);

            // Assert
            _mockMapper.Verify(m => m.Map(updateDto, existingProject), Times.Once);
            _mockUnitOfWork.Verify(u => u.Projects.Update(existingProject), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);

            Assert.Equal(updateDto.Name, existingProject.Name);
            Assert.Equal(updateDto.Description, existingProject.Description);
        }


        [Fact]
        public async System.Threading.Tasks.Task GetAllProjectsAsync_WhenCalled_ReturnsPagedResult()
        {
            // Arrange
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var projects = new List<Project> { new() { Id = 1, Name = "P1" }, new() { Id = 2, Name = "P2" } };
            var projectDtos = new List<ProjectDto> { new() { Id = 1, Name = "P1" }, new() { Id = 2, Name = "P2" } };

            _mockUnitOfWork.Setup(u => u.Projects.GetPagedAsync(1, 10))
                           .ReturnsAsync((projects, 2)); // (Items, TotalCount)
            _mockMapper.Setup(m => m.Map<IEnumerable<ProjectDto>>(projects))
                       .Returns(projectDtos);

            // Act
            var result = await _sut.GetAllProjectsAsync(paginationParams);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.Items.Count());
        }
    }
}
