using MemoriesVietnam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Domain.IRepositories
{
    public interface IEraRepository
    {
        Task<IEnumerable<Era>> GetActiveErasAsync();
        Task<IEnumerable<Era>> GetErasByYearAsync(int year);
    }
}
