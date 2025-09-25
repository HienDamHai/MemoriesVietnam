using MemoriesVietnam.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoriesVietnam.Application.DTOs.OAuthAccountDto;

namespace MemoriesVietnam.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> OAuthLoginAsync(OAuthLoginRequest request);
    }
}
