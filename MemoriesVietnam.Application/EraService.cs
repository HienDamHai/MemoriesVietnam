using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Repositories.Interfaces;
using MemoriesVietnam.Services.Interfaces;

namespace MemoriesVietnam.Services
{
    public class EraService : IEraService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EraService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EraDto>> GetAllAsync()
        {
            var eras = await _unitOfWork.Eras.GetAllAsync();
            return eras.OrderBy(e => e.YearStart).Select(MapToDto).ToList();
        }

        public async Task<EraDto?> GetByIdAsync(string id)
        {
            var era = await _unitOfWork.Eras.GetByIdAsync(id);
            return era != null ? MapToDto(era) : null;
        }

        public async Task<EraDto> CreateAsync(CreateEraDto dto)
        {
            var era = new Era
            {
                Name = dto.Name,
                YearStart = dto.YearStart,
                YearEnd = dto.YearEnd,
                Description = dto.Description
            };

            await _unitOfWork.Eras.AddAsync(era);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(era);
        }

        public async Task<EraDto?> UpdateAsync(string id, UpdateEraDto dto)
        {
            var era = await _unitOfWork.Eras.GetByIdAsync(id);
            if (era == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) era.Name = dto.Name;
            if (dto.YearStart.HasValue) era.YearStart = dto.YearStart.Value;
            if (dto.YearEnd.HasValue) era.YearEnd = dto.YearEnd.Value;
            if (dto.Description != null) era.Description = dto.Description;

            await _unitOfWork.Eras.UpdateAsync(era);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(era);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var era = await _unitOfWork.Eras.GetByIdAsync(id);
            if (era == null) return false;

            // Check if era has articles
            var hasArticles = await _unitOfWork.Articles.ExistsAsync(a => a.EraId == id);
            if (hasArticles)
            {
                throw new Exception("Không thể xóa thời kỳ này vì đã có bài viết liên quan");
            }

            await _unitOfWork.Eras.DeleteAsync(era);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResponse<ArticleDto>> GetArticlesByEraAsync(string eraId, int page, int limit)
        {
            var (articles, total) = await _unitOfWork.Articles.GetPagedAsync(
                page, 
                limit, 
                a => a.EraId == eraId,
                orderBy: q => q.OrderByDescending(a => a.CreatedAt),
                includeProperties: "Era,ArticleAudios,ArticleTags.Tag"
            );

            var articleDtos = articles.Select(MapArticleToDto).ToList();
            
            return new PaginatedResponse<ArticleDto>
            {
                Data = articleDtos,
                Total = total,
                Page = page,
                Limit = limit,
                TotalPages = (int)Math.Ceiling((double)total / limit)
            };
        }

        private EraDto MapToDto(Era era)
        {
            return new EraDto
            {
                Id = era.Id,
                Name = era.Name,
                YearStart = era.YearStart,
                YearEnd = era.YearEnd,
                Description = era.Description
            };
        }

        private ArticleDto MapArticleToDto(Article article)
        {
            return new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Slug = article.Slug,
                Content = article.Content,
                CoverUrl = article.CoverUrl,
                YearStart = article.YearStart,
                YearEnd = article.YearEnd,
                EraId = article.EraId,
                Sources = article.Sources,
                PublishedAt = article.PublishedAt,
                CreatedAt = article.CreatedAt,
                Era = article.Era != null ? MapToDto(article.Era) : null,
                ArticleAudios = article.ArticleAudios?.Select(aa => new ArticleAudioDto
                {
                    Id = aa.Id,
                    ArticleId = aa.ArticleId,
                    VoiceId = aa.VoiceId,
                    Url = aa.Url,
                    Duration = aa.Duration,
                    CreatedBy = aa.CreatedBy
                }).ToList() ?? new List<ArticleAudioDto>(),
                Tags = article.ArticleTags?.Select(at => new TagDto
                {
                    Id = at.Tag.Id,
                    Name = at.Tag.Name,
                    Slug = at.Tag.Slug,
                    CreatedAt = at.Tag.CreatedAt
                }).ToList() ?? new List<TagDto>()
            };
        }
    }
}
