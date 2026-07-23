using TaskManagement.Application.DTOs.Label;
using TaskManagement.Application.DTOs.Common;

namespace TaskManagement.Application.Interfaces.Services;

public interface ILabelService
{
    Task<PagedResult<LabelDto>> GetLabelsAsync(int projectId, PaginationParams paginationParams);
    Task<LabelDto?> GetLabelByIdAsync(int id);
    Task<LabelDto> CreateLabelAsync(CreateLabelDto dto);
    Task UpdateLabelAsync(int id, UpdateLabelDto dto);
    Task DeleteLabelAsync(int id);
}