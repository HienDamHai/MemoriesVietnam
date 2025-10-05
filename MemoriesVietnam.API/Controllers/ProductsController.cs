using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<ProductDto>>>> GetAll([FromQuery] ProductQuery query)
        {
            try
            {
                var products = await _productService.GetAllAsync(query);
                return Ok(new ApiResponse<PaginatedResponse<ProductDto>>
                {
                    Data = products,
                    Message = "Lấy danh sách sản phẩm thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PaginatedResponse<ProductDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(string id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new ApiResponse<ProductDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy sản phẩm",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<ProductDto>
                {
                    Data = product,
                    Message = "Lấy thông tin sản phẩm thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ProductDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetBySlug(string slug)
        {
            try
            {
                var product = await _productService.GetBySlugAsync(slug);
                if (product == null)
                {
                    return NotFound(new ApiResponse<ProductDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy sản phẩm",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<ProductDto>
                {
                    Data = product,
                    Message = "Lấy thông tin sản phẩm thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ProductDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetFeatured()
        {
            try
            {
                var products = await _productService.GetFeaturedAsync();
                return Ok(new ApiResponse<List<ProductDto>>
                {
                    Data = products,
                    Message = "Lấy danh sách sản phẩm nổi bật thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<ProductDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                var product = await _productService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, new ApiResponse<ProductDto>
                {
                    Data = product,
                    Message = "Tạo sản phẩm thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ProductDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> Update(string id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                var product = await _productService.UpdateAsync(id, dto);
                if (product == null)
                {
                    return NotFound(new ApiResponse<ProductDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy sản phẩm",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<ProductDto>
                {
                    Data = product,
                    Message = "Cập nhật sản phẩm thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<ProductDto>
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
                var result = await _productService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = null,
                        Message = "Không tìm thấy sản phẩm",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Data = null,
                    Message = "Xóa sản phẩm thành công",
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
