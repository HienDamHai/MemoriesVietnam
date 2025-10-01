using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IBookmarksRepository
    {
        Task<IEnumerable<Bookmark>> GetBookmarkByUserIdAsync(string userid);

        Task<IEnumerable<Bookmark>> GetBookmarkByTargetAsync(string targetId, TargetType targetType);
    }
}
