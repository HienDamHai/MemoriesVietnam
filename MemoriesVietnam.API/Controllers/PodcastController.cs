using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesVietnam.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PodcastController : ControllerBase
    {
        private readonly PodcastService _podcastService;
        public PodcastController(PodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var podcasts = await _podcastService.GetAllAsync();
            return Ok(podcasts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var podcast = await _podcastService.GetByIdAsync(id);
            if (podcast == null || podcast.IsDeleted)
                return NotFound();
            return Ok(podcast);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PodcastDto.CreatePodcastDto createPodcastDto)
        {
            var podcast = new Podcast
            {
                Title = createPodcastDto.Title,
                Description = createPodcastDto.Description,
                CoverUrl = createPodcastDto.CoverUrl
            };

            var createdPodcast = await _podcastService.CreateAsync(podcast);
            return CreatedAtAction(nameof(GetById), new { id = createdPodcast.Id }, createdPodcast);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PodcastDto.UpdatePodcastDto updatePodcastDto)
        {
            if (id != updatePodcastDto.Id)
                return BadRequest();

            var podcast = new Podcast
            {
                Id = updatePodcastDto.Id,
                Title = updatePodcastDto.Title,
                Description = updatePodcastDto.Description,
                CoverUrl = updatePodcastDto.CoverUrl
            };

            var updatedPodcast = await _podcastService.UpdateAsync(podcast);
            if (updatedPodcast == null)
                return NotFound();
            return Ok(updatedPodcast);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _podcastService.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
