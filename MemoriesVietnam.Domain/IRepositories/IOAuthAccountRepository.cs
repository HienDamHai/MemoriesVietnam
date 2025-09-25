using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IOAuthAccountRepository : IGenericRepository<OAuthAccount>
    {
        Task<OAuthAccount?> GetByIdAsync(string id);
        Task<IEnumerable<OAuthAccount>> GetByLoginIdAsync(string loginId);
        Task<OAuthAccount?> GetByProviderAsync(string provider, string providerUserId);
    }
}
