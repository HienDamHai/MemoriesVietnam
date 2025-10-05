using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using MemoriesVietnam.Configuration;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MemoriesVietnam.Services
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly CloudinarySettings _settings;

        public UploadService(IOptions<CloudinarySettings> settings)
        {
            _settings = settings.Value;
            var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<UploadResponseDto> UploadImageAsync(IFormFile file)
        {
            if (!IsValidImageFile(file))
            {
                throw new Exception("File không hợp lệ. Chỉ chấp nhận file JPG, PNG, GIF");
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "memoirs-vietnam/images",
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Lỗi tải lên: {uploadResult.Error.Message}");
            }

            return new UploadResponseDto
            {
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        public async Task<UploadResponseDto> UploadAudioAsync(IFormFile file)
        {
            if (!IsValidAudioFile(file))
            {
                throw new Exception("File không hợp lệ. Chỉ chấp nhận file MP3, WAV, M4A");
            }

            using var stream = file.OpenReadStream();
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "memoirs-vietnam/audio"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception($"Lỗi tải lên: {uploadResult.Error.Message}");
            }

            return new UploadResponseDto
            {
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        public async Task<bool> DeleteFileAsync(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);
                return result.Result == "ok";
            }
            catch
            {
                return false;
            }
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var mimeType = file.ContentType.ToLowerInvariant();

            return allowedExtensions.Contains(extension) && allowedMimeTypes.Contains(mimeType);
        }

        public bool IsValidAudioFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".mp3", ".wav", ".m4a", ".aac" };
            var allowedMimeTypes = new[] { "audio/mpeg", "audio/wav", "audio/mp4", "audio/aac" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var mimeType = file.ContentType.ToLowerInvariant();

            return allowedExtensions.Contains(extension) && allowedMimeTypes.Contains(mimeType);
        }
    }
}
