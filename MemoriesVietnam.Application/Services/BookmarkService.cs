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
    public class BookmarkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookmarksRepository _bookmarksRepository;

        public BookmarkService(IUnitOfWork unitOfWork, IBookmarksRepository bookmarksRepository)
        {
            _unitOfWork = unitOfWork;
            _bookmarksRepository = bookmarksRepository;
        }

        public async Task<IEnumerable<Bookmark>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Bookmark>().GetAllAsync();
        }

        public async Task<Bookmark> GetBookmarkByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Bookmark>().GetByIdAsync(id);
        }

        public async Task<Bookmark> CreateAsync(Bookmark bookmark)
        {
            await _unitOfWork.Repository<Bookmark>().AddAsync(bookmark);
            await _unitOfWork.SaveChangesAsync();
            return bookmark;
        }

        public async Task<Bookmark?> UpdateAsync(string id, string targetId, TargetType targetType)
        {
            var bookmark = await _unitOfWork.Repository<Bookmark>().GetByIdAsync(id);
            if (bookmark == null || bookmark.IsDeleted) return null;

            bookmark.TargetId = targetId;
            bookmark.TargetType = targetType;

            _unitOfWork.Repository<Bookmark>().Update(bookmark);
            await _unitOfWork.SaveChangesAsync();

            return bookmark;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var bookmark = await _unitOfWork.Repository<Bookmark>().GetByIdAsync(id);
            if (bookmark == null) return false;
            _unitOfWork.Repository<Bookmark>().Remove(bookmark);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Bookmark>> GetBookmarkByUserIdAsync(string userId)
        {
            return await _bookmarksRepository.GetBookmarkByUserIdAsync(userId);
        }

        // 🔹 Get all bookmarks for a target
        public async Task<IEnumerable<Bookmark>> GetBookmarkByTargetAsync(string targetId, TargetType targetType)
        {
            return await _bookmarksRepository.GetBookmarkByTargetAsync(targetId, targetType);
        }

    }
}
