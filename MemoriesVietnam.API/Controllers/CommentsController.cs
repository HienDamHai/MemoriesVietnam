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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CommentDto>>>> GetByTarget(
            [FromQuery] string targetId,
            [FromQuery] CommentTargetType targetType)
        {
            try
            {
                var comments = await _commentService.GetByTargetAsync(targetId, targetType);
                return Ok(new ApiResponse<List<CommentDto>>
                {
                    Data = comments,
                    Message = "Lấy danh sách bình luận thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<CommentDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CommentDto>>> Create([FromBody] CreateCommentDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<CommentDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var comment = await _commentService.CreateAsync(userId, dto);
                return Ok(new ApiResponse<CommentDto>
                {
                    Data = comment,
                    Message = "Tạo bình luận thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CommentDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<CommentDto>>> Update(string id, [FromBody] UpdateCommentDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<CommentDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var comment = await _commentService.UpdateAsync(id, userId, dto);
                if (comment == null)
                {
                    return NotFound(new ApiResponse<CommentDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy bình luận hoặc bạn không có quyền chỉnh sửa",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<CommentDto>
                {
                    Data = comment,
                    Message = "Cập nhật bình luận thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CommentDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Data = null,
                        Message = "Không tìm thấy thông tin người dùng",
                        Success = false
                    });
                }

                var result = await _commentService.DeleteAsync(id, userId);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = null,
                        Message = "Không tìm thấy bình luận hoặc bạn không có quyền xóa",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Data = null,
                    Message = "Xóa bình luận thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
