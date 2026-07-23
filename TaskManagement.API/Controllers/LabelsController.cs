using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Common;
using TaskManagement.Application.DTOs.Label;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetLabels(int projectId, [FromQuery] PaginationParams paginationParams)
        {
            var labels = await _labelService.GetLabelsAsync(projectId, paginationParams);
            return Ok(labels);
        }

        [HttpGet("label/{id}")]
        public async Task<IActionResult> GetLabelById(int id)
        {
            var label = await _labelService.GetLabelByIdAsync(id);
            if (label == null)
            {
                return NotFound();
            }
            return Ok(label);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] CreateLabelDto dto)
        {
            var label = await _labelService.CreateLabelAsync(dto);
            return CreatedAtAction(nameof(GetLabelById), new { id = label.Id }, label);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(int id, [FromBody] UpdateLabelDto dto)
        {
            await _labelService.UpdateLabelAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(int id)
        {
            await _labelService.DeleteLabelAsync(id);
            return NoContent();
        }



    }
}
