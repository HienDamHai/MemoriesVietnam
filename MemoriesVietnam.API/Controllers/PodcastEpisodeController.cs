using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Services;
using MemoriesVietnam.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemoriesVietnam.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PodcastEpisodeController : ControllerBase
    {
        private readonly PodcastEpisodeService _podcastEpisodeService;
        public PodcastEpisodeController(PodcastEpisodeService podcastEpisodeService)
        {
            _podcastEpisodeService = podcastEpisodeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var podcastEpisodes = await _podcastEpisodeService.GetAllAsync();
            return Ok(podcastEpisodes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var podcastEpisode = await _podcastEpisodeService.GetByIdAsync(id);
            if (podcastEpisode == null || podcastEpisode.IsDeleted)
                return NotFound();
            return Ok(podcastEpisode);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PodcastEpisodeDto.CreatePodcastEpisodeDto createPodcastEpisodeDto)
        {
            var podcastEpisode = new PodcastEpisode
            {
                PodcastId = createPodcastEpisodeDto.PodcastId,
                Title = createPodcastEpisodeDto.Title,
                AudioUrl = createPodcastEpisodeDto.AudioUrl,
                Duration = createPodcastEpisodeDto.Duration,
                EpisodeNumber = createPodcastEpisodeDto.EpisodeNumber,
                ArticleId = createPodcastEpisodeDto.ArticleId
            };
            var createdPodcastEpisode = await _podcastEpisodeService.CreateAsync(podcastEpisode);
            return CreatedAtAction(nameof(GetById), new { id = createdPodcastEpisode.Id }, createdPodcastEpisode);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PodcastEpisodeDto.UpdatePodcastEpisodeDto updatePodcastEpisodeDto)
        {
            if (id != updatePodcastEpisodeDto.Id)
                return BadRequest();
            var podcastEpisode = new PodcastEpisode
            {
                Id = updatePodcastEpisodeDto.Id,
                PodcastId = updatePodcastEpisodeDto.PodcastId,
                Title = updatePodcastEpisodeDto.Title,
                AudioUrl = updatePodcastEpisodeDto.AudioUrl,
                Duration = updatePodcastEpisodeDto.Duration,
                EpisodeNumber = updatePodcastEpisodeDto.EpisodeNumber,
                ArticleId = updatePodcastEpisodeDto.ArticleId
            };
            var updatedPodcastEpisode = await _podcastEpisodeService.UpdateAsync(podcastEpisode);
            if (updatedPodcastEpisode == null)
                return NotFound();
            return Ok(updatedPodcastEpisode);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _podcastEpisodeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
