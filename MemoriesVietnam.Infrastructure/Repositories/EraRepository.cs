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
    public class EraRepository : GenericRepository<Era>, IEraRepository
    {
        private readonly AppDbContext _context;

        public EraRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Era>> GetActiveErasAsync()
        {
            return await _context.Eras
                .Where(e => !e.IsDeleted)
                .Include(e => e.Articles)
                .ToListAsync();
        }

        public async Task<IEnumerable<Era>> GetErasByYearAsync(int year)
        {
            return await _context.Eras
                .Where(e => !e.IsDeleted && e.YearStart <= year && e.YearEnd >= year)
                .Include(e => e.Articles)
                .ToListAsync();
        }
    }
}
