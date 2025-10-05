using MemoriesVietnam.Models.DTOs;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserDto?> GetCurrentUserAsync(string userId);
        string GenerateJwtToken(string userId, string email, string role);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
