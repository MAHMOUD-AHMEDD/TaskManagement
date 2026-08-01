using AutoMapper;
using Moq;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.Comment;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests
{
    public class CommentServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CommentService _sut; // "sut" = System Under Test

        public CommentServiceTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _sut = new CommentService(_mockUnitOfWork.Object, _mockMapper.Object);

        }


        [Fact]
        public async System.Threading.Tasks.Task GetCommentByIdAsync_WhenCommentExists_ReturnsCommentDto()
        {
            // Arrange
            var comment = new Domain.Entities.Comment { Id = 1, Content = "Test Comment", TaskId = 1 };
            var expectedDto = new Application.DTOs.Comment.CommentDto { Id = 1, Content = "Test Comment" };
            _mockUnitOfWork.Setup(u => u.Comments.GetByIdAsync(1)).ReturnsAsync(comment);
            _mockMapper.Setup(m => m.Map<Application.DTOs.Comment.CommentDto>(comment)).Returns(expectedDto);

            // Act
            var result = await _sut.GetCommentByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Content, result.Content);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCommentByIdAsync_WhenCommentDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Comments.GetByIdAsync(1)).ReturnsAsync((Comment?)null);
            //Act
            var result = await _sut.GetCommentByIdAsync(1);
            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async System.Threading.Tasks.Task DeleteCommentAsync_WhenCommentExists_DeletesCommentAndSavesChanges()
        {
            // Arrange
            var comment = new Domain.Entities.Comment { Id = 1, Content = "Test Comment", TaskId = 1 };
            _mockUnitOfWork.Setup(u => u.Comments.GetByIdAsync(1)).ReturnsAsync(comment);
            //Act
            await _sut.DeleteCommentAsync(1);
            // Assert
            _mockUnitOfWork.Verify(u => u.Comments.Delete(comment), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async System.Threading.Tasks.Task DeleteCommentAsync_WhenCommentDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Comments.GetByIdAsync(1)).ReturnsAsync((Comment?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.DeleteCommentAsync(1));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateCommentAsync_WhenCalled_CreatesCommentAndSavesChanges()
        {
            // Arrange
            var UserId = "user123";
            var commentDto = new Application.DTOs.Comment.CreateCommentDto { Content = "Test Comment", TaskId = 1 };
            var comment = new Comment { Id = 1, Content = "Test Comment", TaskId = 1 };
            _mockMapper.Setup(m => m.Map<Comment>(commentDto)).Returns(comment);
            _mockUnitOfWork.Setup(u => u.Comments.AddAsync(comment));
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await _sut.CreateCommentAsync(commentDto, UserId);
            // Assert
            _mockUnitOfWork.Verify(u => u.Comments.AddAsync(comment), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateCommentAsync_WhenCommentExists_UpdatesCommentAndSavesChanges()
        {
            // Arrange
            var comment = new Domain.Entities.Comment { Id = 1, Content = "Old Comment", TaskId = 1 };
            var updateDto = new Application.DTOs.Comment.UpdateCommentDto { Content = "Updated Comment" };
            _mockUnitOfWork.Setup(u => u.Comments.GetByIdAsync(1)).ReturnsAsync(comment);
            _mockMapper.Setup(m => m.Map(updateDto, comment)).Returns(comment);

            // Act
            await _sut.UpdateCommentAsync(1, updateDto);

            // Assert
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateCommentAsync_WhenCommentDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var updateDto = new Application.DTOs.Comment.UpdateCommentDto { Content = "Updated Comment" };
            _mockUnitOfWork.Setup(u => u.Comments.GetByIdAsync(1)).ReturnsAsync((Comment?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _sut.UpdateCommentAsync(1, updateDto));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetCommentsByTaskIdAsync_WhenCalled_ReturnsPagedResult()
        {
            // Arrange
            var taskId = 1;
            var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "Comment 1", TaskId = taskId },
                new Comment { Id = 2, Content = "Comment 2", TaskId = taskId }
            };
            var totalCount = comments.Count;
            _mockUnitOfWork.Setup(u => u.Comments.GetPagedAsync(
                 It.IsAny<Expression<Func<Comment, bool>>>(),
                paginationParams.PageNumber,
                paginationParams.PageSize))
                .ReturnsAsync((comments, totalCount));
            var commentDtos = comments.Select(c => new CommentDto { Id = c.Id, Content = c.Content });
            _mockMapper.Setup(m => m.Map<IEnumerable<CommentDto>>(comments)).Returns(commentDtos);
            // Act
            var result = await _sut.GetCommentsAsync(taskId, paginationParams);
            // Assert
            Assert.Equal(totalCount, result.TotalCount);
            Assert.Equal(paginationParams.PageNumber, result.PageNumber);
            Assert.Equal(paginationParams.PageSize, result.PageSize);
            Assert.Equal(commentDtos.Count(), result.Items.Count());


        }
    }
}