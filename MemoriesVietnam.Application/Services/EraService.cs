using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class EraService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEraRepository _eraRepository;

        public EraService(IUnitOfWork unitOfWork, IEraRepository eraRepository)
        {
            _unitOfWork = unitOfWork;
            _eraRepository = eraRepository;
        }

        public async Task<IEnumerable<Era>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Era>().GetAllAsync();
        }

        public async Task<IEnumerable<Era>> GetActiveAsync()
        {
            return await _eraRepository.GetActiveErasAsync();
        }

        public async Task<IEnumerable<Era>> GetByYearAsync(int year)
        {
            return await _eraRepository.GetErasByYearAsync(year);
        }

        public async Task<Era?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Era>().GetByIdAsync(id);
        }

        public async Task<Era> CreateAsync(Era era)
        {
            await _unitOfWork.Repository<Era>().AddAsync(era);
            await _unitOfWork.SaveChangesAsync();
            return era;
        }

        public async Task<Era?> UpdateAsync(string id, string name, int yearStart, int yearEnd, string description)
        {
            var era = await _unitOfWork.Repository<Era>().GetByIdAsync(id);
            if (era == null || era.IsDeleted) return null;

            era.Name = name;
            era.YearStart = yearStart;
            era.YearEnd = yearEnd;
            era.Description = description;

            _unitOfWork.Repository<Era>().Update(era);
            await _unitOfWork.SaveChangesAsync();

            return era;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var era = await _unitOfWork.Repository<Era>().GetByIdAsync(id);
            if (era == null || era.IsDeleted) return false;

            era.IsDeleted = true;
            era.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Era>().Update(era);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
