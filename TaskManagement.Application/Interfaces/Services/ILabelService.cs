using TaskManagement.Application.DTOs.Label;

namespace TaskManagement.Application.Interfaces.Services;

public interface ILabelService
{
    Task<IEnumerable<LabelDto>> GetLabelsAsync(int projectId);
    Task<LabelDto?> GetLabelByIdAsync(int id);
    Task<LabelDto> CreateLabelAsync(CreateLabelDto dto);
    Task UpdateLabelAsync(int id, UpdateLabelDto dto);
    Task DeleteLabelAsync(int id);
}