using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.DTOs.Auth;
using MemoriesVietnam.Application.Interfaces;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemoriesVietnam.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IAuthRepository _authRepository;
        private readonly IOAuthAccountRepository _oAuthAccountRepository;

        public AuthService(IOAuthAccountRepository oAuthAccountRepository, IConfiguration config, IUnitOfWork unitOfWork, IAuthRepository authRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _authRepository = authRepository;
            _oAuthAccountRepository = oAuthAccountRepository;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var Logins = await _unitOfWork.Repository<Login>()
                .GetAllAsync();
            var existing = await _authRepository.GetByEmailAsync(request.Email);

            if (existing != null)
                return new AuthResponse { Success = false, Message = "Email already registered" };

            var login = new Login
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = LoginRole.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Login>().AddAsync(login);

            var user = new User
            {
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                LoginId = login.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<User>().AddAsync(user);

            await _unitOfWork.SaveChangesAsync();


            return new AuthResponse
            {
                Success = true,
                Email = login.Email,
                Role = login.Role.ToString(),
                Token = GenerateJwtToken(login, user.Id),
                Message = "Registration successful"
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var login = await _authRepository.GetByEmailAsync(request.Email);
            if (login == null || !BCrypt.Net.BCrypt.Verify(request.Password, login.PasswordHash))
                throw new Exception("Invalid credentials");

            var userId = login.Users.FirstOrDefault()?.Id;

            return new AuthResponse
            {
                Success = true,
                Email = login.Email,
                Role = login.Role.ToString(),
                Token = GenerateJwtToken(login, userId),
                Message = "Login successful"
            };
        }

        private string GenerateJwtToken(Login login, string userId)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", userId),
                new Claim(JwtRegisteredClaimNames.Sub, login.Id),
                new Claim(ClaimTypes.Email, login.Email ?? ""),
                new Claim(ClaimTypes.Role, login.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponse> OAuthLoginAsync(OAuthAccountDto.OAuthLoginRequest request)
        {
            var oauthRepo = _unitOfWork.Repository<OAuthAccount>();
            var existing = await _oAuthAccountRepository.GetByProviderAsync(request.Provider, request.ProviderUserId);
            Login login;
            User user;

            if (existing != null)
            {
                existing.AccessToken = request.AccessToken;
                existing.RefreshToken = request.RefreshToken;
                existing.ExpireAt = request.ExpireAt;

                login = await _unitOfWork.Repository<Login>().GetByIdAsync(existing.LoginId);

                // Tìm User theo LoginId
                var users = await _unitOfWork.Repository<User>().GetAllAsync();
                user = users.FirstOrDefault(u => u.LoginId == login.Id);

                if (user == null)
                {
                    throw new Exception("User not found for this login");
                }

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                login = new Login
                {
                    Email = request.Email ?? $"{request.ProviderUserId}@{request.Provider.ToLower()}.oauth",
                    PasswordHash = null,
                    Role = LoginRole.User,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<Login>().AddAsync(login);

                user = new User
                {
                    Name = request.Name ?? request.ProviderUserId,
                    Phone = "N/A",
                    Address = "N/A",
                    LoginId = login.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<User>().AddAsync(user);

                var oauthAccount = new OAuthAccount
                {
                    Provider = request.Provider,
                    ProviderUserId = request.ProviderUserId,
                    AccessToken = request.AccessToken,
                    RefreshToken = request.RefreshToken,
                    ExpireAt = request.ExpireAt,
                    LoginId = login.Id,
                };

                await oauthRepo.AddAsync(oauthAccount);

                await _unitOfWork.SaveChangesAsync();
            }

            return new AuthResponse
            {
                Success = true,
                Email = login.Email,
                Role = login.Role.ToString(),
                Token = GenerateJwtToken(login, user.Id),
                Message = "Login successful"
            };
        }
    }
}
