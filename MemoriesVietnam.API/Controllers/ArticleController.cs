using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MemoriesVietnam.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _articleService.GetAllAsync();
            var dtos = articles.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Slug = a.Slug,
                Content = a.Content,
                CoverUrl = a.CoverUrl,
                YearStart = a.YearStart,
                YearEnd = a.YearEnd,
                EraId = a.EraId,
                Sources = a.Sources,
                PublishedAt = a.PublishedAt
            });
            return Ok(dtos);
        }

        [HttpGet("published")]
        public async Task<IActionResult> GetPublished()
        {
            var articles = await _articleService.GetPublishedAsync();
            return Ok(articles);
        }

        [HttpGet("era/{eraId}")]
        public async Task<IActionResult> GetByEra(string eraId)
        {
            var articles = await _articleService.GetByEraIdAsync(eraId);
            return Ok(articles);
        }

        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            var articles = await _articleService.GetByYearAsync(year);
            return Ok(articles);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var article = await _articleService.GetBySlugAsync(slug);
            if (article == null) return NotFound();
            return Ok(article);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null) return NotFound();

            return Ok(new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Slug = article.Slug,
                Content = article.Content,
                CoverUrl = article.CoverUrl,
                YearStart = article.YearStart,
                YearEnd = article.YearEnd,
                EraId = article.EraId,
                Sources = article.Sources,
                PublishedAt = article.PublishedAt
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticleDto dto)
        {
            var article = new Article
            {
                Title = dto.Title,
                Slug = dto.Slug,
                Content = dto.Content,
                CoverUrl = dto.CoverUrl,
                YearStart = dto.YearStart,
                YearEnd = dto.YearEnd,
                EraId = dto.EraId,
                Sources = dto.Sources,
                PublishedAt = dto.PublishedAt
            };

            var created = await _articleService.CreateAsync(article);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateArticleDto dto)
        {
            var updated = await _articleService.UpdateAsync(id, dto.Title, dto.Slug, dto.Content, dto.CoverUrl, dto.YearStart, dto.YearEnd, dto.EraId, dto.Sources, dto.PublishedAt);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _articleService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
