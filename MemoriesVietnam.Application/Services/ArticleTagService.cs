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
    public class ArticleTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleTagRepository _articleTagRepository;

        public ArticleTagService(IUnitOfWork unitOfWork, IArticleTagRepository articleTagRepository)
        {
            _unitOfWork = unitOfWork;
            _articleTagRepository = articleTagRepository;
        }

        public async Task<IEnumerable<ArticleTag>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ArticleTag>().GetAllAsync();
        }

        public async Task<ArticleTag?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<ArticleTag>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<ArticleTag>> GetByArticleIdAsync(string articleId)
        {
            return await _articleTagRepository.GetByArticleIdAsync(articleId);
        }

        public async Task<IEnumerable<ArticleTag>> GetByTagIdAsync(string tagId)
        {
            return await _articleTagRepository.GetByTagIdAsync(tagId);
        }

        public async Task<ArticleTag> CreateAsync(ArticleTag articleTag)
        {
            await _unitOfWork.Repository<ArticleTag>().AddAsync(articleTag);
            await _unitOfWork.SaveChangesAsync();
            return articleTag;
        }

        public async Task<ArticleTag?> UpdateAsync(string id, string articleId, string tagId)
        {
            var entity = await _unitOfWork.Repository<ArticleTag>().GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return null;

            entity.ArticleId = articleId;
            entity.TagId = tagId;

            _unitOfWork.Repository<ArticleTag>().Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _unitOfWork.Repository<ArticleTag>().GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return false;

            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ArticleTag>().Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
