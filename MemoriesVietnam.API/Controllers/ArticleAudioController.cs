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
    public class ArticleAudioController : ControllerBase
    {
        private readonly ArticleAudioService _articleAudioService;

        public ArticleAudioController(ArticleAudioService articleAudioService)
        {
            _articleAudioService = articleAudioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var audios = await _articleAudioService.GetAllAsync();
            var dtos = audios.Select(a => new ArticleAudioDto
            {
                Id = a.Id,
                ArticleId = a.ArticleId,
                VoiceId = a.VoiceId,
                Url = a.Url,
                Duration = a.Duration,
                CreatedBy = a.CreatedBy
            });
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var audio = await _articleAudioService.GetByIdAsync(id);
            if (audio == null) return NotFound();

            return Ok(new ArticleAudioDto
            {
                Id = audio.Id,
                ArticleId = audio.ArticleId,
                VoiceId = audio.VoiceId,
                Url = audio.Url,
                Duration = audio.Duration,
                CreatedBy = audio.CreatedBy
            });
        }

        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetByArticleId(string articleId)
        {
            var audios = await _articleAudioService.GetByArticleIdAsync(articleId);
            var dtos = audios.Select(a => new ArticleAudioDto
            {
                Id = a.Id,
                ArticleId = a.ArticleId,
                VoiceId = a.VoiceId,
                Url = a.Url,
                Duration = a.Duration,
                CreatedBy = a.CreatedBy
            });
            return Ok(dtos);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var audios = await _articleAudioService.GetByUserIdAsync(userId);
            var dtos = audios.Select(a => new ArticleAudioDto
            {
                Id = a.Id,
                ArticleId = a.ArticleId,
                VoiceId = a.VoiceId,
                Url = a.Url,
                Duration = a.Duration,
                CreatedBy = a.CreatedBy
            });
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArticleAudioDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var audio = new ArticleAudio
            {
                ArticleId = dto.ArticleId,
                VoiceId = dto.VoiceId,
                Url = dto.Url,
                Duration = dto.Duration,
                CreatedBy = userId
            };

            var created = await _articleAudioService.CreateAsync(audio);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ArticleAudioDto
            {
                Id = created.Id,
                ArticleId = created.ArticleId,
                VoiceId = created.VoiceId,
                Url = created.Url,
                Duration = created.Duration,
                CreatedBy = created.CreatedBy
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateArticleAudioDto dto)
        {
            var updated = await _articleAudioService.UpdateAsync(id, dto.VoiceId, dto.Url, dto.Duration);
            if (updated == null) return NotFound();

            return Ok(new ArticleAudioDto
            {
                Id = updated.Id,
                ArticleId = updated.ArticleId,
                VoiceId = updated.VoiceId,
                Url = updated.Url,
                Duration = updated.Duration,
                CreatedBy = updated.CreatedBy
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _articleAudioService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        private string GetUserId()
        {
            return User.FindFirst("userId")?.Value ?? "";
        }
    }
}
