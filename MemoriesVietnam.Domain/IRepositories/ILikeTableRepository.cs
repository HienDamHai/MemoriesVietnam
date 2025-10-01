using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface ILikeTableRepository : IGenericRepository<LikeTable>
    {
        Task<IEnumerable<LikeTable>> GetByUserIdAsync(string userId);
        Task<IEnumerable<LikeTable>> GetByTargetAsync(string targetId, TargetType targetType);
        Task<LikeTable?> GetUserLikeAsync(string userId, string targetId, TargetType targetType);
    }
}
