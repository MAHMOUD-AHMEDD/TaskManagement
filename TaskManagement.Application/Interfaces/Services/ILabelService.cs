using TaskManagement.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ILabelService
    {
        Task<ICollection<Label>>GetLabelsAsync();
        Task GetLabelByIdAsync(int id);
        Task UpdateLabelAsync(int LabelId, int ProjectId, string name, string color);
        Task DeleteLabelAsync(int LabelId);
        Task CreateLabelAsync(int ProjectId, string name, string color);
    }
}
