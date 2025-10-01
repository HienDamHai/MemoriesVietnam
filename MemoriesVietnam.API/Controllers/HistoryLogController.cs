using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.DTOs.MemoriesVietnam.Application.DTOs;
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
    public class HistoryLogController : ControllerBase
    {
        private readonly HistoryLogService _historyLogService;

        public HistoryLogController(HistoryLogService historyLogService)
        {
            _historyLogService = historyLogService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var logs = await _historyLogService.GetByUserAsync(userId);
            return Ok(logs.Select(h => new HistoryLogDto
            {
                Id = h.Id,
                UserId = h.UserId,
                TargetId = h.TargetId,
                TargetType = h.TargetType,
                Progress = h.Progress,
                CreatedAt = h.CreatedAt
            }));
        }

        [HttpGet("target/{targetId}/{targetType}")]
        public async Task<IActionResult> GetByTarget(string targetId, TargetType targetType)
        {
            var logs = await _historyLogService.GetByTargetAsync(targetId, targetType);
            return Ok(logs);
        }

        [HttpGet("latest/{userId}/{targetId}/{targetType}")]
        public async Task<IActionResult> GetLatest(string userId, string targetId, TargetType targetType)
        {
            var log = await _historyLogService.GetLatestAsync(userId, targetId, targetType);
            if (log == null) return NotFound();
            return Ok(log);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHistoryLogDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var log = new HistoryLog
            {
                UserId = userId,
                TargetId = dto.TargetId,
                TargetType = dto.TargetType,
                Progress = dto.Progress
            };

            var created = await _historyLogService.CreateAsync(log);

            return CreatedAtAction(nameof(GetLatest), new { userId = userId, targetId = dto.TargetId, targetType = dto.TargetType }, created);
        }

        private string GetUserId()
        {
            return User.FindFirst("userId")?.Value ?? "";
        }
    }
}
