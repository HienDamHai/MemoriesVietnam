using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PodcastsController : ControllerBase
    {
        private readonly IPodcastService _podcastService;

        public PodcastsController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PodcastDto>>>> GetAll()
        {
            try
            {
                var podcasts = await _podcastService.GetAllAsync();
                return Ok(new ApiResponse<List<PodcastDto>>
                {
                    Data = podcasts,
                    Message = "Lấy danh sách podcast thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<PodcastDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PodcastDto>>> GetById(string id)
        {
            try
            {
                var podcast = await _podcastService.GetByIdAsync(id);
                if (podcast == null)
                {
                    return NotFound(new ApiResponse<PodcastDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy podcast",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<PodcastDto>
                {
                    Data = podcast,
                    Message = "Lấy thông tin podcast thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PodcastDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("{id}/episodes")]
        public async Task<ActionResult<ApiResponse<List<PodcastEpisodeDto>>>> GetEpisodes(string id)
        {
            try
            {
                var episodes = await _podcastService.GetEpisodesByPodcastAsync(id);
                return Ok(new ApiResponse<List<PodcastEpisodeDto>>
                {
                    Data = episodes,
                    Message = "Lấy danh sách tập podcast thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<PodcastEpisodeDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpGet("episodes/latest")]
        public async Task<ActionResult<ApiResponse<List<PodcastEpisodeDto>>>> GetLatestEpisodes()
        {
            try
            {
                var episodes = await _podcastService.GetLatestEpisodesAsync();
                return Ok(new ApiResponse<List<PodcastEpisodeDto>>
                {
                    Data = episodes,
                    Message = "Lấy danh sách tập podcast mới nhất thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<PodcastEpisodeDto>>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<PodcastDto>>> Create([FromBody] CreatePodcastDto dto)
        {
            try
            {
                var podcast = await _podcastService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = podcast.Id }, new ApiResponse<PodcastDto>
                {
                    Data = podcast,
                    Message = "Tạo podcast thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PodcastDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost("{id}/episodes")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<PodcastEpisodeDto>>> CreateEpisode(string id, [FromBody] CreatePodcastEpisodeDto dto)
        {
            try
            {
                var episode = await _podcastService.CreateEpisodeAsync(id, dto);
                return Ok(new ApiResponse<PodcastEpisodeDto>
                {
                    Data = episode,
                    Message = "Tạo tập podcast thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PodcastEpisodeDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<ActionResult<ApiResponse<PodcastDto>>> Update(string id, [FromBody] CreatePodcastDto dto)
        {
            try
            {
                var podcast = await _podcastService.UpdateAsync(id, dto);
                if (podcast == null)
                {
                    return NotFound(new ApiResponse<PodcastDto>
                    {
                        Data = null,
                        Message = "Không tìm thấy podcast",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<PodcastDto>
                {
                    Data = podcast,
                    Message = "Cập nhật podcast thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PodcastDto>
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
                var result = await _podcastService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = null,
                        Message = "Không tìm thấy podcast",
                        Success = false
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Data = null,
                    Message = "Xóa podcast thành công",
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
