using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IRepositories;
using MemoriesVietnam.Infrastructure.Basic;
using MemoriesVietnam.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Infrastructure.Repositories
{
    public class OAuthAccountRepository: GenericRepository<OAuthAccount>, IOAuthAccountRepository
    {
        private readonly AppDbContext _context;
        public OAuthAccountRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<OAuthAccount?> GetByIdAsync(string id)
        {
            return await _context.OAuthAccounts.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
        public async Task<IEnumerable<OAuthAccount>> GetByLoginIdAsync(string loginId)
        {
            return await _context.OAuthAccounts
                .Where(a => a.LoginId == loginId && !a.IsDeleted)
                .ToListAsync();
        }
        public async Task<OAuthAccount?> GetByProviderAsync(string provider, string providerUserId)
        {
            return await _context.OAuthAccounts
                .FirstOrDefaultAsync(a => a.Provider == provider 
                && a.ProviderUserId == providerUserId
                && !a.IsDeleted);
        }
    }
}
