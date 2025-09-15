using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Entities;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IAuthRepository
    {
        Task<Login> GetByEmailAsync(string email);
        Task AddLoginAsync(Login login);
    }
}
