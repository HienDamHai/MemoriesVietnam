using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Application.Interfaces;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Phone = u.Phone,
                Address = u.Address,
                Email = u.Login?.Email ?? "",
                VerifiedAt = u.VerifiedAt
            });
        }

        public async Task<UserDto?> GetByIdWithLoginAsync(string id)
        {
            var user = await _userRepository.GetByIdWithLoginAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                Email = user.Login?.Email ?? "",
                VerifiedAt = user.VerifiedAt
            };
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                Email = user.Login?.Email ?? "",
                VerifiedAt = user.VerifiedAt
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserRequest request)
        {
            var login = new Login
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = LoginRole.User
            };

            var user = new User
            {
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                Login = login
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.Repository<Login>().AddAsync(login);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                Email = login.Email,
                VerifiedAt = user.VerifiedAt
            };
        }

        public async Task<bool> UpdateAsync(string id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;
            user.Name = request.Name;
            user.Phone = request.Phone;
            user.Address = request.Address;
            user.UpdatedAt = DateTime.UtcNow;
            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;
            _userRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdWithLoginAsync(userId);
            if (user == null || user.Login == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Login.PasswordHash))
                return false;

            user.Login.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.Now;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
