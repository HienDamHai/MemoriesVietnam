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
    public class CommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentRepository _commentRepository;

        public CommentService(IUnitOfWork unitOfWork, ICommentRepository commentRepository)
        {
            _unitOfWork = unitOfWork;
            _commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Comment>> GetByTargetAsync(string targetId, TargetType targetType)
        {
            return await _commentRepository.GetByTargetAsync(targetId, targetType);
        }

        public async Task<IEnumerable<Comment>> GetByUserAsync(string userId)
        {
            return await _commentRepository.GetByUserAsync(userId);
        }

        public async Task<IEnumerable<Comment>> GetRepliesAsync(string parentId)
        {
            return await _commentRepository.GetRepliesAsync(parentId);
        }

        public async Task<Comment?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Comment>().GetByIdAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _unitOfWork.Repository<Comment>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(string id, string content, string? imageUrl)
        {
            var comment = await _unitOfWork.Repository<Comment>().GetByIdAsync(id);
            if (comment == null || comment.IsDeleted) return null;

            comment.Content = content;
            comment.ImageUrl = imageUrl;

            _unitOfWork.Repository<Comment>().Update(comment);
            await _unitOfWork.SaveChangesAsync();

            return comment;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var comment = await _unitOfWork.Repository<Comment>().GetByIdAsync(id);
            if (comment == null || comment.IsDeleted) return false;

            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Comment>().Update(comment);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
