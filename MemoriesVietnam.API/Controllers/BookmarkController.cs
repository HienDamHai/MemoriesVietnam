using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MemoriesVietnam.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly BookmarkService _bookmarkService;

        public BookmarkController(BookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        // GET: api/bookmark
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookmarks = await _bookmarkService.GetAllAsync();
            var dtos = bookmarks.Select(b => new BookmarkDto
            {
                Id = b.Id,
                UserId = b.UserId,
                TargetId = b.TargetId,
                TargetType = b.TargetType,
                CreatedAt = b.CreatedAt
            });
            return Ok(dtos);
        }

        // GET: api/bookmark/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var bookmark = await _bookmarkService.GetBookmarkByIdAsync(id);
            if (bookmark == null) return NotFound();

            var dto = new BookmarkDto
            {
                Id = bookmark.Id,
                UserId = bookmark.UserId,
                TargetId = bookmark.TargetId,
                TargetType = bookmark.TargetType,
                CreatedAt = bookmark.CreatedAt
            };
            return Ok(dto);
        }

        // POST: api/bookmark
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookmarkDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var bookmark = new Bookmark
            {
                UserId = userId,
                TargetId = dto.TargetId,
                TargetType = dto.TargetType
            };

            var created = await _bookmarkService.CreateAsync(bookmark);

            var result = new BookmarkDto
            {
                Id = created.Id,
                UserId = created.UserId,
                TargetId = created.TargetId,
                TargetType = created.TargetType,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/bookmark/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateBookmarkDto dto)
        {
            var updated = await _bookmarkService.UpdateAsync(id, dto.TargetId, dto.TargetType);
            if (updated == null) return NotFound();

            var result = new BookmarkDto
            {
                Id = updated.Id,
                UserId = updated.UserId,
                TargetId = updated.TargetId,
                TargetType = updated.TargetType,
                CreatedAt = updated.CreatedAt
            };

            return Ok(result);
        }

        // DELETE: api/bookmark/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _bookmarkService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // GET: api/bookmark/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMyBookmarks()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var bookmarks = await _bookmarkService.GetBookmarkByUserIdAsync(userId);

            var dtos = bookmarks.Select(b => new BookmarkDto
            {
                Id = b.Id,
                UserId = b.UserId,
                TargetId = b.TargetId,
                TargetType = b.TargetType,
                CreatedAt = b.CreatedAt
            });

            return Ok(dtos);
        }


        // GET: api/bookmark/target/{targetId}/{targetType}
        [HttpGet("target/{targetId}/{targetType}")]
        public async Task<IActionResult> GetByTarget(string targetId, TargetType targetType)
        {
            var bookmarks = await _bookmarkService.GetBookmarkByTargetAsync(targetId, targetType);

            var dtos = bookmarks.Select(b => new BookmarkDto
            {
                Id = b.Id,
                UserId = b.UserId,
                TargetId = b.TargetId,
                TargetType = b.TargetType,
                CreatedAt = b.CreatedAt
            });

            return Ok(dtos);
        }

        private string GetUserId()
        {
            return User.FindFirst("userId")?.Value ?? "";
        }
    }
}
