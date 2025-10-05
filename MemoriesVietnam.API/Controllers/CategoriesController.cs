using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductService _productService;

        public CategoriesController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAll()
        {
            try
            {
                var categories = await _productService.GetCategoriesAsync();
                return Ok(new ApiResponse<List<CategoryDto>>
                {
                    Data = categories,
                    Message = "Lấy danh sách danh mục thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<CategoryDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> Create([FromBody] CreateTagDto dto)
        {
            try
            {
                var category = await _productService.CreateCategoryAsync(dto.Name);
                return Ok(new ApiResponse<CategoryDto>
                {
                    Data = category,
                    Message = "Tạo danh mục thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<CategoryDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
