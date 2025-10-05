using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UploadsController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadsController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("image")]
        public async Task<ActionResult<ApiResponse<UploadResponseDto>>> UploadImage(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    return BadRequest(new ApiResponse<UploadResponseDto>
                    {
                        Data = null,
                        Message = "Vui lòng chọn file hình ảnh",
                        Success = false
                    });
                }

                if (!_uploadService.IsValidImageFile(image))
                {
                    return BadRequest(new ApiResponse<UploadResponseDto>
                    {
                        Data = null,
                        Message = "File không hợp lệ. Chỉ chấp nhận file JPG, PNG, GIF",
                        Success = false
                    });
                }

                var result = await _uploadService.UploadImageAsync(image);
                return Ok(new ApiResponse<UploadResponseDto>
                {
                    Data = result,
                    Message = "Tải lên hình ảnh thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UploadResponseDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }

        [HttpPost("audio")]
        public async Task<ActionResult<ApiResponse<UploadResponseDto>>> UploadAudio(IFormFile audio)
        {
            try
            {
                if (audio == null || audio.Length == 0)
                {
                    return BadRequest(new ApiResponse<UploadResponseDto>
                    {
                        Data = null,
                        Message = "Vui lòng chọn file âm thanh",
                        Success = false
                    });
                }

                if (!_uploadService.IsValidAudioFile(audio))
                {
                    return BadRequest(new ApiResponse<UploadResponseDto>
                    {
                        Data = null,
                        Message = "File không hợp lệ. Chỉ chấp nhận file MP3, WAV, M4A",
                        Success = false
                    });
                }

                var result = await _uploadService.UploadAudioAsync(audio);
                return Ok(new ApiResponse<UploadResponseDto>
                {
                    Data = result,
                    Message = "Tải lên file âm thanh thành công",
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<UploadResponseDto>
                {
                    Data = null,
                    Message = ex.Message,
                    Success = false
                });
            }
        }
    }
}
