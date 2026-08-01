using AutoMapper;
using Moq;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Label;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests
{
    public class LabelServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly LabelService _sut; // "sut" = System Under Test

        public LabelServiceTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _sut = new LabelService(_mockUnitOfWork.Object, _mockMapper.Object);

        }

        [Fact]
        public async System.Threading.Tasks.Task GetLabelByIdAsync_WhenLabelExists_ReturnsLabelDto()
        {
            // Arrange
            var label = new Label { Id = 1, Name = "Test Label" };
            var expectedDto = new LabelDto { Id = 1, Name = "Test Label" };
            _mockUnitOfWork.Setup(u => u.Labels.GetByIdAsync(1)).ReturnsAsync(label);
            _mockMapper.Setup(m => m.Map<LabelDto>(label)).Returns(expectedDto);

            // Act
            var result = await _sut.GetLabelByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);

        }

        [Fact]
        public async System.Threading.Tasks.Task GetLabelByIdAsync_WhenLabelDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Labels.GetByIdAsync(1)).ReturnsAsync((Label?)null);
            //Act
            var result = await _sut.GetLabelByIdAsync(1);
            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteLabelAsync_WhenLabelExists_DeletesLabelAndSavesChanges()
        {
            // Arrange
            var label = new Label { Id = 1, Name = "Test Label" };
            _mockUnitOfWork.Setup(u => u.Labels.GetByIdAsync(1)).ReturnsAsync(label);
            //act
            await _sut.DeleteLabelAsync(1);
            //Assert
            _mockUnitOfWork.Verify(u => u.Labels.Delete(label), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteLabelAsync_WhenLabelDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Labels.GetByIdAsync(1)).ReturnsAsync((Label?)null);
            //Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteLabelAsync(1));

        }

        [Fact]
        public async System.Threading.Tasks.Task CreateLabelAsync_WhenCalled_CreatesLabelAndSavesChanges()
        {
            // Arrange
            var createDto = new CreateLabelDto { Name = "New Label" };
            var label = new Label { Id = 1, Name = "New Label" };
            var expectedDto = new LabelDto { Id = 1, Name = "New Label" };
            _mockMapper.Setup(m => m.Map<Label>(createDto)).Returns(label);
            _mockUnitOfWork.Setup(u => u.Labels.AddAsync(label));
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<LabelDto>(label)).Returns(expectedDto);
            // Act
            var result = await _sut.CreateLabelAsync(createDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
            _mockUnitOfWork.Verify(u => u.Labels.AddAsync(label), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateLabelAsync_WhenLabelExists_UpdatesLabelAndSavesChanges()
        {
            // Arrange
            var label = new Label { Id = 1, Name = "Old Label" };
            var updateDto = new UpdateLabelDto { Name = "Updated Label" };
            _mockUnitOfWork.Setup(u => u.Labels.GetByIdAsync(1)).ReturnsAsync(label);
            // Act
            await _sut.UpdateLabelAsync(1, updateDto);
            // Assert
            _mockMapper.Verify(m => m.Map(updateDto, label), Times.Once);
            _mockUnitOfWork.Verify(u => u.Labels.Update(label), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateLabelAsync_WhenLabelDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var updateDto = new UpdateLabelDto { Name = "Updated Label" };
            _mockUnitOfWork.Setup(u => u.Labels.GetByIdAsync(1)).ReturnsAsync((Label?)null);
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateLabelAsync(1, updateDto));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllLabelsByProjectIdAsync_WhenCalled_ReturnsPagedResult()
        {
            // Arrange
            var projectId = 1;
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var labels = new List<Label>
            {
                new Label { Id = 1, Name = "Label 1", ProjectId = projectId },
                new Label { Id = 2, Name = "Label 2", ProjectId = projectId }
            };
            var expectedDtos = new List<LabelDto>
            {
                new LabelDto { Id = 1, Name = "Label 1", ProjectId = projectId },
                new LabelDto { Id = 2, Name = "Label 2", ProjectId = projectId }
            };

            _mockUnitOfWork.Setup(u => u.Labels.GetPagedAsync(
                                It.IsAny<Expression<Func<Label, bool>>>(),
                                paginationParams.PageNumber,
                                paginationParams.PageSize))
                           .ReturnsAsync((labels, 2));

            _mockMapper.Setup(m => m.Map<IEnumerable<LabelDto>>(labels)).Returns(expectedDtos);

            // Act
            var result = await _sut.GetLabelsAsync(projectId, paginationParams);

            // Assert
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(expectedDtos.Count, result.Items.Count());
            Assert.Equal(paginationParams.PageNumber, result.PageNumber);
            Assert.Equal(paginationParams.PageSize, result.PageSize);
        }
    }
}
