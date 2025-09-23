using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IRepositories;
using MemoriesVietnam.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MemoriesVietnam.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLoginAsync(Domain.Entities.Login login)
        {
            await _context.Logins.AddAsync(login);
            await _context.SaveChangesAsync();
        }

        public async Task<Login> GetByEmailAsync(string email)
        {
            return await _context.Logins.Include(l => l.Users).FirstOrDefaultAsync(l => l.Email == email);
        }
    }
}
