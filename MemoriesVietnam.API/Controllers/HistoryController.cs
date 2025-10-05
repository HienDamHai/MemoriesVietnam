using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;
using System.Security.Claims;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<HistoryLogDto>>>> GetMyHistory()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<List<HistoryLogDto>>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var history = await _historyService.GetMyHistoryAsync(userId);
                return Ok(new ApiResponse<List<HistoryLogDto>>
                {
                    Data = history,
                    Message = "Lấy lịch sử thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<HistoryLogDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<HistoryLogDto>>> CreateOrUpdate([FromBody] CreateHistoryLogDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<HistoryLogDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var history = await _historyService.CreateOrUpdateAsync(userId, dto);
                return Ok(new ApiResponse<HistoryLogDto>
                {
                    Data = history,
                    Message = "Cập nhật lịch sử thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<HistoryLogDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
