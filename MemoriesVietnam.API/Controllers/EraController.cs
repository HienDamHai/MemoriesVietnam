using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MemoriesVietnam.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EraController : ControllerBase
    {
        private readonly EraService _eraService;

        public EraController(EraService eraService)
        {
            _eraService = eraService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var eras = await _eraService.GetAllAsync();
            var dtos = eras.Select(e => new EraDto
            {
                Id = e.Id,
                Name = e.Name,
                YearStart = e.YearStart,
                YearEnd = e.YearEnd,
                Description = e.Description
            });
            return Ok(dtos);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var eras = await _eraService.GetActiveAsync();
            var dtos = eras.Select(e => new EraDto
            {
                Id = e.Id,
                Name = e.Name,
                YearStart = e.YearStart,
                YearEnd = e.YearEnd,
                Description = e.Description
            });
            return Ok(dtos);
        }

        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            var eras = await _eraService.GetByYearAsync(year);
            var dtos = eras.Select(e => new EraDto
            {
                Id = e.Id,
                Name = e.Name,
                YearStart = e.YearStart,
                YearEnd = e.YearEnd,
                Description = e.Description
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var era = await _eraService.GetByIdAsync(id);
            if (era == null) return NotFound();

            return Ok(new EraDto
            {
                Id = era.Id,
                Name = era.Name,
                YearStart = era.YearStart,
                YearEnd = era.YearEnd,
                Description = era.Description
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEraDto dto)
        {
            var era = new Era
            {
                Name = dto.Name,
                YearStart = dto.YearStart,
                YearEnd = dto.YearEnd,
                Description = dto.Description
            };

            var created = await _eraService.CreateAsync(era);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new EraDto
            {
                Id = created.Id,
                Name = created.Name,
                YearStart = created.YearStart,
                YearEnd = created.YearEnd,
                Description = created.Description
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateEraDto dto)
        {
            var updated = await _eraService.UpdateAsync(id, dto.Name, dto.YearStart, dto.YearEnd, dto.Description);
            if (updated == null) return NotFound();

            return Ok(new EraDto
            {
                Id = updated.Id,
                Name = updated.Name,
                YearStart = updated.YearStart,
                YearEnd = updated.YearEnd,
                Description = updated.Description
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _eraService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
