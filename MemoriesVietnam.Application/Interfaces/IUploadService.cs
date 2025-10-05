using MemoriesVietnam.Models.DTOs;
using Microsoft.AspNetCore.Http;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IUploadService
    {
        Task<UploadResponseDto> UploadImageAsync(IFormFile file);
        Task<UploadResponseDto> UploadAudioAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string publicId);
        bool IsValidImageFile(IFormFile file);
        bool IsValidAudioFile(IFormFile file);
    }
}
