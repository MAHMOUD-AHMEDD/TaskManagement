using AutoMapper;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Label;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class LabelService : ILabelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LabelDto?> GetLabelByIdAsync(int id)
        {
            var label = await _unitOfWork.Labels.GetByIdAsync(id);
            return label == null ? null : _mapper.Map<LabelDto>(label);
        }

        public async Task<PagedResult<LabelDto>> GetLabelsAsync(int projectId, PaginationParams paginationParams)
        {
            var (labels, totalCount) = await _unitOfWork.Labels.GetPagedAsync(
                l => l.ProjectId == projectId,
                paginationParams.PageNumber,
                paginationParams.PageSize
            );
            var labelDtos = _mapper.Map<IEnumerable<LabelDto>>(labels);

            return new PagedResult<LabelDto>
            {
                Items = labelDtos,
                TotalCount = totalCount,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
        }

        public async Task<LabelDto> CreateLabelAsync(CreateLabelDto dto)
        {
            var label = _mapper.Map<Domain.Entities.Label>(dto);
            await _unitOfWork.Labels.AddAsync(label);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<LabelDto>(label);
        }

        public async Task UpdateLabelAsync(int id, UpdateLabelDto dto)
        {
            var label = await _unitOfWork.Labels.GetByIdAsync(id);
            if (label == null)
            {
                throw new KeyNotFoundException($"Label with ID {id} not found.");
            }
            _mapper.Map(dto, label);
            _unitOfWork.Labels.Update(label);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteLabelAsync(int id)
        {
            var label = await _unitOfWork.Labels.GetByIdAsync(id);
            if (label == null)
            {
                throw new KeyNotFoundException($"Label with ID {id} not found.");
            }
            _unitOfWork.Labels.Delete(label);
            await _unitOfWork.SaveChangesAsync();
        }



    }
}
