using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ICommentService
    {

        Task<ICollection<Comment>> GetCommentsAsync();
        Task GetCommentByIdAsync(int id);
        Task CreateCommentAsync(int userId, int TaskId, string content);
        Task UpdateCommentAsync(int id, string content);
        Task DeleteCommentAsync(int id);


    }
}
