using MemoriesVietnam.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdWithLoginAsync(string id);
        Task<UserDto?> GetByIdAsync(string id);
        Task<UserDto> CreateAsync(CreateUserRequest request);
        Task<bool> UpdateAsync(string id, UpdateUserRequest request);
        Task<bool> DeleteAsync(string id);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
