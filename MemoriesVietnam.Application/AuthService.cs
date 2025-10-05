using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MemoriesVietnam.Configuration;
using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Repositories.Interfaces;
using MemoriesVietnam.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MemoriesVietnam.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Kiểm tra email đã tồn tại
            var existingLogin = await _unitOfWork.Logins.FindFirstAsync(l => l.Email == request.Email);
            if (existingLogin != null)
            {
                throw new Exception("Email đã được sử dụng");
            }

            // Tạo Login
            var login = new Login
            {
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = UserRole.User
            };

            await _unitOfWork.Logins.AddAsync(login);
            await _unitOfWork.SaveChangesAsync();

            // Tạo User
            var user = new User
            {
                LoginId = login.Id,
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Tạo JWT token
            var token = GenerateJwtToken(user.Id, login.Email, login.Role.ToString());

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = login.Email,
                    Role = login.Role,
                    Phone = user.Phone,
                    Address = user.Address,
                    VerifiedAt = user.VerifiedAt,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var login = await _unitOfWork.Logins.FindFirstAsync(l => l.Email == request.Email);
            if (login == null || string.IsNullOrEmpty(login.PasswordHash) || !VerifyPassword(request.Password, login.PasswordHash))
            {
                throw new Exception("Email hoặc mật khẩu không đúng");
            }

            var user = await _unitOfWork.Users.FindFirstAsync(u => u.LoginId == login.Id);
            if (user == null)
            {
                throw new Exception("Không tìm thấy thông tin người dùng");
            }

            var token = GenerateJwtToken(user.Id, login.Email, login.Role.ToString());

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = login.Email,
                    Role = login.Role,
                    Phone = user.Phone,
                    Address = user.Address,
                    VerifiedAt = user.VerifiedAt,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                }
            };
        }

        public async Task<UserDto?> GetCurrentUserAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return null;

            var login = await _unitOfWork.Logins.GetByIdAsync(user.LoginId);
            if (login == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = login.Email,
                Role = login.Role,
                Phone = user.Phone,
                Address = user.Address,
                VerifiedAt = user.VerifiedAt,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public string GenerateJwtToken(string userId, string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            var hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var testHash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != testHash[i])
                    return false;
            }
            return true;
        }
    }
}
