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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("target/{targetId}/{targetType}")]
        public async Task<IActionResult> GetByTarget(string targetId, TargetType targetType)
        {
            var comments = await _commentService.GetByTargetAsync(targetId, targetType);
            return Ok(comments);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var comments = await _commentService.GetByUserAsync(userId);
            return Ok(comments);
        }

        [HttpGet("replies/{parentId}")]
        public async Task<IActionResult> GetReplies(string parentId)
        {
            var replies = await _commentService.GetRepliesAsync(parentId);
            return Ok(replies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var comment = new Comment
            {
                UserId = userId,
                TargetId = dto.TargetId,
                TargetType = dto.TargetType,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                ParentId = dto.ParentId
            };

            var created = await _commentService.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateCommentDto dto)
        {
            var updated = await _commentService.UpdateAsync(id, dto.Content, dto.ImageUrl);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _commentService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        private string GetUserId()
        {
            return User.FindFirst("userId")?.Value ?? "";
        }
    }
}
