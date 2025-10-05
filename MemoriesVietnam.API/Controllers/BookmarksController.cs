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
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<BookmarkDto>>>> GetMyBookmarks()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<List<BookmarkDto>>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var bookmarks = await _bookmarkService.GetMyBookmarksAsync(userId);
                return Ok(new ApiResponse<List<BookmarkDto>>
                {
                    Data = bookmarks,
                    Message = "Lấy danh sách bookmark thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<BookmarkDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost("toggle")]
        public async Task<ActionResult<ApiResponse<BookmarkResponseDto>>> Toggle([FromBody] BookmarkToggleDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<BookmarkResponseDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var result = await _bookmarkService.ToggleAsync(userId, dto);
                return Ok(new ApiResponse<BookmarkResponseDto>
                {
                    Data = result,
                    Message = result.Bookmarked ? "Đã lưu bookmark" : "Đã bỏ bookmark",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<BookmarkResponseDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("check")]
        public async Task<ActionResult<ApiResponse<BookmarkStatusDto>>> Check(
            [FromQuery] string targetId,
            [FromQuery] BookmarkTargetType targetType)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<BookmarkStatusDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var status = await _bookmarkService.CheckAsync(targetId, targetType, userId);
                return Ok(new ApiResponse<BookmarkStatusDto>
                {
                    Data = status,
                    Message = "Kiểm tra trạng thái bookmark thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<BookmarkStatusDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
