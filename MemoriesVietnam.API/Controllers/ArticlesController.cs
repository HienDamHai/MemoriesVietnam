using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ArticleDto>>>> GetAll([FromQuery] ArticleQuery query)
        {
            try
            {
                var articles = await _articleService.GetAllAsync(query);
                return Ok(new ApiResponse<PaginatedResponse<ArticleDto>>
                {
                    Data = articles,
                    Message = "Lấy danh sách bài viết thành công",
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ArticleDto>>> GetById(string id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(id);
                if (article == null)
                {
                    return NotFound(new ApiResponse<ArticleDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy bài viết",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<ArticleDto>
                {
                    Data = article,
                    Message = "Lấy thông tin bài viết thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ArticleDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<ApiResponse<ArticleDto>>> GetBySlug(string slug)
        {
            try
            {
                var article = await _articleService.GetBySlugAsync(slug);
                if (article == null)
                {
                    return NotFound(new ApiResponse<ArticleDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy bài viết",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<ArticleDto>
                {
                    Data = article,
                    Message = "Lấy thông tin bài viết thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ArticleDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ApiResponse<List<ArticleDto>>>> GetFeatured()
        {
            try
            {
                var articles = await _articleService.GetFeaturedAsync();
                return Ok(new ApiResponse<List<ArticleDto>>
                {
                    Data = articles,
                    Message = "Lấy danh sách bài viết nổi bật thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ArticleDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ArticleDto>>>> Search(
            [FromQuery] string q,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            try
            {
                var articles = await _articleService.SearchAsync(q, page, limit);
                return Ok(new ApiResponse<PaginatedResponse<ArticleDto>>
                {
                    Data = articles,
                    Message = "Tìm kiếm bài viết thành công",
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
        public async Task<ActionResult<ApiResponse<ArticleDto>>> Create([FromBody] CreateArticleDto dto)
        {
            try
            {
                var article = await _articleService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = article.Id }, new ApiResponse<ArticleDto>
                {
                    Data = article,
                    Message = "Tạo bài viết thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ArticleDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<ArticleDto>>> Update(string id, [FromBody] UpdateArticleDto dto)
        {
            try
            {
                var article = await _articleService.UpdateAsync(id, dto);
                if (article == null)
                {
                    return NotFound(new ApiResponse<ArticleDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy bài viết",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<ArticleDto>
                {
                    Data = article,
                    Message = "Cập nhật bài viết thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ArticleDto>
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
                var result = await _articleService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = null,
                        Message = "Không tìm thấy bài viết",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Data = null,
                    Message = "Xóa bài viết thành công",
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
