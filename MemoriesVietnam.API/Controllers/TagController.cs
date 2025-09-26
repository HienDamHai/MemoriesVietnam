using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesVietnam.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly TagService _tagService;
        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null || tag.IsDeleted) return NotFound();
            return Ok(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTagDto dto)
        {
            var tag = new Tag
            {
                Name = dto.Name,
                Slug = dto.Slug
            };

            var createdTag = await _tagService.AddAsync(tag);
            return CreatedAtAction(nameof(GetById), new { id = createdTag.Id }, createdTag);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTagDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");
            var tag = new Tag
            {
                Id = dto.Id,
                Name = dto.Name,
                Slug = dto.Slug
            };
            var updatedTag = await _tagService.UpdateAsync(tag);
            if (updatedTag == null) return NotFound();
            return Ok(updatedTag);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _tagService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
