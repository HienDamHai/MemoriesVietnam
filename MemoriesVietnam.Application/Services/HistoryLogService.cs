using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class HistoryLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHistoryLogRepository _historyLogRepository;

        public HistoryLogService(IUnitOfWork unitOfWork, IHistoryLogRepository historyLogRepository)
        {
            _unitOfWork = unitOfWork;
            _historyLogRepository = historyLogRepository;
        }

        public async Task<IEnumerable<HistoryLog>> GetAllAsync()
        {
            return await _unitOfWork.Repository<HistoryLog>().GetAllAsync();
        }

        public async Task<IEnumerable<HistoryLog>> GetByUserAsync(string userId)
        {
            return await _historyLogRepository.GetByUserAsync(userId);
        }

        public async Task<IEnumerable<HistoryLog>> GetByTargetAsync(string targetId, TargetType targetType)
        {
            return await _historyLogRepository.GetByTargetAsync(targetId, targetType);
        }

        public async Task<HistoryLog?> GetLatestAsync(string userId, string targetId, TargetType targetType)
        {
            return await _historyLogRepository.GetLatestAsync(userId, targetId, targetType);
        }

        public async Task<HistoryLog> CreateAsync(HistoryLog log)
        {
            await _unitOfWork.Repository<HistoryLog>().AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
            return log;
        }
    }
}
