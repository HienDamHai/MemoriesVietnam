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
    public interface IHistoryLogRepository : IGenericRepository<HistoryLog>
    {
        Task<IEnumerable<HistoryLog>> GetByUserAsync(string userId);
        Task<IEnumerable<HistoryLog>> GetByTargetAsync(string targetId, TargetType targetType);
        Task<HistoryLog?> GetLatestAsync(string userId, string targetId, TargetType targetType);
    }
}
