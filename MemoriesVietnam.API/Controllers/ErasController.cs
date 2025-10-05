using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErasController : ControllerBase
    {
        private readonly IEraService _eraService;

        public ErasController(IEraService eraService)
        {
            _eraService = eraService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<EraDto>>>> GetAll()
        {
            try
            {
                var eras = await _eraService.GetAllAsync();
                return Ok(new ApiResponse<List<EraDto>>
                {
                    Data = eras,
                    Message = "Lấy danh sách thời kỳ thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<EraDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<EraDto>>> GetById(string id)
        {
            try
            {
                var era = await _eraService.GetByIdAsync(id);
                if (era == null)
                {
                    return NotFound(new ApiResponse<EraDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thời kỳ",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<EraDto>
                {
                    Data = era,
                    Message = "Lấy thông tin thời kỳ thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<EraDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}/articles")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ArticleDto>>>> GetArticlesByEra(
            string id, 
            [FromQuery] int page = 1, 
            [FromQuery] int limit = 10)
        {
            try
            {
                var articles = await _eraService.GetArticlesByEraAsync(id, page, limit);
                return Ok(new ApiResponse<PaginatedResponse<ArticleDto>>
                {
                    Data = articles,
                    Message = "Lấy danh sách bài viết theo thời kỳ thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PaginatedResponse<ArticleDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<EraDto>>> Create([FromBody] CreateEraDto dto)
        {
            try
            {
                var era = await _eraService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = era.Id }, new ApiResponse<EraDto>
                {
                    Data = era,
                    Message = "Tạo thời kỳ thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<EraDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<EraDto>>> Update(string id, [FromBody] UpdateEraDto dto)
        {
            try
            {
                var era = await _eraService.UpdateAsync(id, dto);
                if (era == null)
                {
                    return NotFound(new ApiResponse<EraDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy thời kỳ",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<EraDto>
                {
                    Data = era,
                    Message = "Cập nhật thời kỳ thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<EraDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(string id)
        {
            try
            {
                var result = await _eraService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = null,
                        Message = "Không tìm thấy thời kỳ",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Data = null,
                    Message = "Xóa thời kỳ thành công",
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
