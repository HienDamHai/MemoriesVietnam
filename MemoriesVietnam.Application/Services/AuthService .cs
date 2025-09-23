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

        public AuthService(IConfiguration config, IUnitOfWork unitOfWork, IAuthRepository authRepository)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _authRepository = authRepository;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var Logins = await _unitOfWork.Repository<Login>()
                .GetAllAsync();
            var existing = Logins.FirstOrDefault(l => l.Email == request.Email);

            if (existing != null)
                return new AuthResponse { Success = false, Message = "Email already registered" };

            var login = new Login
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = LoginRole.User
            };

            await _unitOfWork.Repository<Login>().AddAsync(login);

            var user = new User
            {
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                LoginId = login.Id
            };

            await _unitOfWork.Repository<User>().AddAsync(user);

            await _unitOfWork.SaveChangesAsync();


            return new AuthResponse
            {
                Success = true,
                Email = login.Email,
                Role = login.Role.ToString(),
                Token = GenerateJwtToken(login),
                Message = "Registration successful"
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var login = await _authRepository.GetByEmailAsync(request.Email);
            if (login == null || !BCrypt.Net.BCrypt.Verify(request.Password, login.PasswordHash))
                throw new Exception("Invalid credentials");

            return new AuthResponse
            {
                Success = true,
                Email = login.Email,
                Role = login.Role.ToString(),
                Token = GenerateJwtToken(login),
                Message = "Login successful"
            };
        }

        private string GenerateJwtToken(Login login)
        {
            var userId = login.Users.FirstOrDefault().Id;

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
    }
}
