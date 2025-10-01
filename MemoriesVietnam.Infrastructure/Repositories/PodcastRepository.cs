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
    public class PodcastRepository : GenericRepository<Podcast>, IPodcastRepository
    {
        private readonly AppDbContext _appDbContext;
        public PodcastRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Podcast>> GetAllWithEpisodesAsync()
        {
            return await _appDbContext.Podcasts
                .Include(x => x.Episodes)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Podcast?> GetByIdWithEpisodesAsync(string id)
        {
            return await _appDbContext.Podcasts
                .Include(p => p.Episodes)
                .Where(p => !p.IsDeleted && p.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
