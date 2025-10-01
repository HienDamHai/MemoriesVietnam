using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IPodcastRepository : IGenericRepository<Podcast>
    {
        Task<IEnumerable<Podcast>> GetAllWithEpisodesAsync();
        Task<Podcast?> GetByIdWithEpisodesAsync(string id);
    }
}
