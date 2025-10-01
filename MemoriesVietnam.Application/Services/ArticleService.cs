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
    public class ArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IUnitOfWork unitOfWork, IArticleRepository articleRepository)
        {
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Article>().GetAllAsync();
        }

        public async Task<Article?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Article>().GetByIdAsync(id);
        }

        public async Task<Article?> GetBySlugAsync(string slug)
        {
            return await _articleRepository.GetBySlugAsync(slug);
        }

        public async Task<IEnumerable<Article>> GetByEraIdAsync(string eraId)
        {
            return await _articleRepository.GetByEraIdAsync(eraId);
        }

        public async Task<IEnumerable<Article>> GetByYearAsync(int year)
        {
            return await _articleRepository.GetByYearRangeAsync(year);
        }

        public async Task<IEnumerable<Article>> GetPublishedAsync()
        {
            return await _articleRepository.GetPublishedAsync();
        }

        public async Task<Article> CreateAsync(Article article)
        {
            await _unitOfWork.Repository<Article>().AddAsync(article);
            await _unitOfWork.SaveChangesAsync();
            return article;
        }

        public async Task<Article?> UpdateAsync(string id, string title, string slug, string content, string coverUrl, int yearStart, int yearEnd, string eraId, string? sources, DateTime? publishedAt)
        {
            var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id);
            if (article == null || article.IsDeleted) return null;

            article.Title = title;
            article.Slug = slug;
            article.Content = content;
            article.CoverUrl = coverUrl;
            article.YearStart = yearStart;
            article.YearEnd = yearEnd;
            article.EraId = eraId;
            article.Sources = sources;
            article.PublishedAt = publishedAt;

            _unitOfWork.Repository<Article>().Update(article);
            await _unitOfWork.SaveChangesAsync();

            return article;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var article = await _unitOfWork.Repository<Article>().GetByIdAsync(id);
            if (article == null || article.IsDeleted) return false;

            article.IsDeleted = true;
            article.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Article>().Update(article);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
