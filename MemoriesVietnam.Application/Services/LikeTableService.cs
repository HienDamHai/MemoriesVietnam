using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class LikeTableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILikeTableRepository _likeRepository;

        public LikeTableService(IUnitOfWork unitOfWork, ILikeTableRepository likeRepository)
        {
            _unitOfWork = unitOfWork;
            _likeRepository = likeRepository;
        }

        public async Task<IEnumerable<LikeTable>> GetByUserIdAsync(string userId)
        {
            return await _likeRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<LikeTable>> GetByTargetAsync(string targetId, TargetType targetType)
        {
            return await _likeRepository.GetByTargetAsync(targetId, targetType);
        }

        public async Task<LikeTable?> GetUserLikeAsync(string userId, string targetId, TargetType targetType)
        {
            return await _likeRepository.GetUserLikeAsync(userId, targetId, targetType);
        }

        public async Task<LikeTable> CreateAsync(LikeTable like)
        {
            await _unitOfWork.Repository<LikeTable>().AddAsync(like);
            await _unitOfWork.SaveChangesAsync();
            return like;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var like = await _unitOfWork.Repository<LikeTable>().GetByIdAsync(id);
            if (like == null || like.IsDeleted) return false;

            like.IsDeleted = true;
            like.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<LikeTable>().Update(like);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
