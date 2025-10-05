using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Services.Interfaces;
using System.Security.Claims;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("toggle")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<LikeResponseDto>>> Toggle([FromBody] LikeToggleDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<LikeResponseDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var result = await _likeService.ToggleAsync(userId, dto);
                return Ok(new ApiResponse<LikeResponseDto>
                {
                    Data = result,
                    Message = result.Liked ? "Đã thích" : "Đã bỏ thích",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<LikeResponseDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<LikeStatusDto>>> GetStatus(
            [FromQuery] string targetId,
            [FromQuery] TargetType targetType)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var status = await _likeService.GetStatusAsync(targetId, targetType, userId);
                
                return Ok(new ApiResponse<LikeStatusDto>
                {
                    Data = status,
                    Message = "Lấy trạng thái thích thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<LikeStatusDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
