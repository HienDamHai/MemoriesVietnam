using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MemoriesVietnam.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto product)
        {
            var item = new Product
            {
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Images = JsonSerializer.Serialize(product.Images),
                CategoryId = product.CategoryId
            };
            var created = await _productService.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto product)
        {
            if (id != product.Id) return BadRequest("ID mismatch");
            var item = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Images = JsonSerializer.Serialize(product.Images),
                CategoryId = product.CategoryId
            };
            var updated = await _productService.UpdateAsync(item);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
