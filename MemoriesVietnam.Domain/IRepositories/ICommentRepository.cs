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
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByTargetAsync(string targetId, TargetType targetType);
        Task<IEnumerable<Comment>> GetByUserAsync(string userId);
        Task<IEnumerable<Comment>> GetRepliesAsync(string parentId);
    }
}
