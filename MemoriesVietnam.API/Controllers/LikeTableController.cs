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
    public class LikeController : ControllerBase
    {
        private readonly LikeTableService _likeService;

        public LikeController(LikeTableService likeService)
        {
            _likeService = likeService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var likes = await _likeService.GetByUserIdAsync(userId);
            return Ok(likes.Select(l => new LikeTableDto
            {
                Id = l.Id,
                UserId = l.UserId,
                TargetId = l.TargetId,
                TargetType = l.TargetType,
                CreatedAt = l.CreatedAt
            }));
        }

        [HttpGet("target/{targetId}/{targetType}")]
        public async Task<IActionResult> GetByTarget(string targetId, TargetType targetType)
        {
            var likes = await _likeService.GetByTargetAsync(targetId, targetType);
            return Ok(likes.Select(l => new LikeTableDto
            {
                Id = l.Id,
                UserId = l.UserId,
                TargetId = l.TargetId,
                TargetType = l.TargetType,
                CreatedAt = l.CreatedAt
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLikeTableDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // prevent duplicate likes
            var existing = await _likeService.GetUserLikeAsync(userId, dto.TargetId, dto.TargetType);
            if (existing != null) return Conflict("Already liked.");

            var like = new LikeTable
            {
                UserId = userId,
                TargetId = dto.TargetId,
                TargetType = dto.TargetType
            };

            var created = await _likeService.CreateAsync(like);

            return CreatedAtAction(nameof(GetByTarget), new { targetId = created.TargetId, targetType = created.TargetType }, new LikeTableDto
            {
                Id = created.Id,
                UserId = created.UserId,
                TargetId = created.TargetId,
                TargetType = created.TargetType,
                CreatedAt = created.CreatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _likeService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        private string GetUserId()
        {
            return User.FindFirst("userId")?.Value ?? "";
        }
    }
}
