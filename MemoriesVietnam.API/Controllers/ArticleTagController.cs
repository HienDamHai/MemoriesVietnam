using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MemoriesVietnam.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleTagController : ControllerBase
    {
        private readonly ArticleTagService _articleTagService;

        public ArticleTagController(ArticleTagService articleTagService)
        {
            _articleTagService = articleTagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _articleTagService.GetAllAsync();
            return Ok(tags.Select(at => new ArticleTagDto
            {
                Id = at.Id,
                ArticleId = at.ArticleId,
                TagId = at.TagId
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var tag = await _articleTagService.GetByIdAsync(id);
            if (tag == null) return NotFound();

            return Ok(new ArticleTagDto
            {
                Id = tag.Id,
                ArticleId = tag.ArticleId,
                TagId = tag.TagId
            });
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetByArticle(string articleId)
        {
            var tags = await _articleTagService.GetByArticleIdAsync(articleId);
            return Ok(tags.Select(at => new ArticleTagDto
            {
                Id = at.Id,
                ArticleId = at.ArticleId,
                TagId = at.TagId
            }));
        }

        [HttpGet("tag/{tagId}")]
        public async Task<IActionResult> GetByTag(string tagId)
        {
            var tags = await _articleTagService.GetByTagIdAsync(tagId);
            return Ok(tags.Select(at => new ArticleTagDto
            {
                Id = at.Id,
                ArticleId = at.ArticleId,
                TagId = at.TagId
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticleTagDto dto)
        {
            var tag = new ArticleTag
            {
                ArticleId = dto.ArticleId,
                TagId = dto.TagId
            };

            var created = await _articleTagService.CreateAsync(tag);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ArticleTagDto
            {
                Id = created.Id,
                ArticleId = created.ArticleId,
                TagId = created.TagId
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _articleTagService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateArticleTagDto dto)
        {
            var updated = await _articleTagService.UpdateAsync(id, dto.ArticleId, dto.TagId);
            if (updated == null) return NotFound();

            return Ok(new ArticleTagDto
            {
                Id = updated.Id,
                ArticleId = updated.ArticleId,
                TagId = updated.TagId
            });
        }

    }
}
