using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IArticleAudioRepository : IGenericRepository<ArticleAudio>
    {
        Task<IEnumerable<ArticleAudio>> GetByArticleIdAsync(string articleId);
        Task<IEnumerable<ArticleAudio>> GetByUserIdAsync(string userId);
    }
}
