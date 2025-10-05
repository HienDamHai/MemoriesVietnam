using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Repositories.Interfaces;
using MemoriesVietnam.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MemoriesVietnam.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResponse<ArticleDto>> GetAllAsync(ArticleQuery query)
        {
            var filter = BuildFilter(query);
            var (articles, total) = await _unitOfWork.Articles.GetPagedAsync(
                query.Page, 
                query.Limit, 
                filter,
                orderBy: q => q.OrderByDescending(a => a.CreatedAt),
                includeProperties: "Era,ArticleAudios,ArticleTags.Tag"
            );

            var articleDtos = articles.Select(MapToDto).ToList();
            
            return new PaginatedResponse<ArticleDto>
            {
                Data = articleDtos,
                Total = total,
                Page = query.Page,
                Limit = query.Limit,
                TotalPages = (int)Math.Ceiling((double)total / query.Limit)
            };
        }

        public async Task<ArticleDto?> GetByIdAsync(string id)
        {
            var article = await _unitOfWork.Articles.FindFirstAsync(a => a.Id == id);
            return article != null ? MapToDto(article) : null;
        }

        public async Task<ArticleDto?> GetBySlugAsync(string slug)
        {
            var article = await _unitOfWork.Articles.FindFirstAsync(a => a.Slug == slug);
            return article != null ? MapToDto(article) : null;
        }

        public async Task<List<ArticleDto>> GetFeaturedAsync()
        {
            var articles = await _unitOfWork.Articles.FindAsync(a => a.PublishedAt != null);
            return articles.Take(6).Select(MapToDto).ToList();
        }

        public async Task<PaginatedResponse<ArticleDto>> SearchAsync(string query, int page, int limit)
        {
            var filter = BuildSearchFilter(query);
            var (articles, total) = await _unitOfWork.Articles.GetPagedAsync(
                page, 
                limit, 
                filter,
                orderBy: q => q.OrderByDescending(a => a.CreatedAt),
                includeProperties: "Era,ArticleAudios,ArticleTags.Tag"
            );

            var articleDtos = articles.Select(MapToDto).ToList();
            
            return new PaginatedResponse<ArticleDto>
            {
                Data = articleDtos,
                Total = total,
                Page = page,
                Limit = limit,
                TotalPages = (int)Math.Ceiling((double)total / limit)
            };
        }

        public async Task<ArticleDto> CreateAsync(CreateArticleDto dto)
        {
            var article = new Article
            {
                Title = dto.Title,
                Slug = GenerateSlug(dto.Title),
                Content = dto.Content,
                CoverUrl = dto.CoverUrl,
                YearStart = dto.YearStart,
                YearEnd = dto.YearEnd,
                EraId = dto.EraId,
                Sources = dto.Sources?.ToString(),
                PublishedAt = DateTime.UtcNow
            };

            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveChangesAsync();

            // Add tags
            if (dto.TagIds?.Any() == true)
            {
                var articleTags = dto.TagIds.Select(tagId => new ArticleTag
                {
                    ArticleId = article.Id,
                    TagId = tagId
                }).ToList();

                await _unitOfWork.ArticleTags.AddRangeAsync(articleTags);
                await _unitOfWork.SaveChangesAsync();
            }

            return MapToDto(article);
        }

        public async Task<ArticleDto?> UpdateAsync(string id, UpdateArticleDto dto)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article == null) return null;

            if (!string.IsNullOrEmpty(dto.Title))
            {
                article.Title = dto.Title;
                article.Slug = GenerateSlug(dto.Title);
            }
            if (!string.IsNullOrEmpty(dto.Content)) article.Content = dto.Content;
            if (dto.CoverUrl != null) article.CoverUrl = dto.CoverUrl;
            if (dto.YearStart.HasValue) article.YearStart = dto.YearStart.Value;
            if (dto.YearEnd.HasValue) article.YearEnd = dto.YearEnd.Value;
            if (!string.IsNullOrEmpty(dto.EraId)) article.EraId = dto.EraId;
            if (dto.Sources != null) article.Sources = dto.Sources.ToString();

            article.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Articles.UpdateAsync(article);

            // Update tags if provided
            if (dto.TagIds != null)
            {
                var existingTags = await _unitOfWork.ArticleTags.FindAsync(at => at.ArticleId == id);
                await _unitOfWork.ArticleTags.DeleteRangeAsync(existingTags);

                if (dto.TagIds.Any())
                {
                    var newTags = dto.TagIds.Select(tagId => new ArticleTag
                    {
                        ArticleId = id,
                        TagId = tagId
                    }).ToList();

                    await _unitOfWork.ArticleTags.AddRangeAsync(newTags);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return MapToDto(article);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var article = await _unitOfWork.Articles.GetByIdAsync(id);
            if (article == null) return false;

            await _unitOfWork.Articles.DeleteAsync(article);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResponse<ArticleDto>> GetByEraAsync(string eraId, int page, int limit)
        {
            var (articles, total) = await _unitOfWork.Articles.GetPagedAsync(
                page, 
                limit, 
                a => a.EraId == eraId,
                orderBy: q => q.OrderByDescending(a => a.CreatedAt),
                includeProperties: "Era,ArticleAudios,ArticleTags.Tag"
            );

            var articleDtos = articles.Select(MapToDto).ToList();
            
            return new PaginatedResponse<ArticleDto>
            {
                Data = articleDtos,
                Total = total,
                Page = page,
                Limit = limit,
                TotalPages = (int)Math.Ceiling((double)total / limit)
            };
        }

        public string GenerateSlug(string title)
        {
            var slug = title.ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');
            return slug;
        }

        private System.Linq.Expressions.Expression<Func<Article, bool>>? BuildFilter(ArticleQuery query)
        {
            System.Linq.Expressions.Expression<Func<Article, bool>>? filter = null;

            if (!string.IsNullOrEmpty(query.EraId))
            {
                filter = a => a.EraId == query.EraId;
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                var searchFilter = BuildSearchFilter(query.Search);
                filter = filter == null ? searchFilter : 
                    a => filter.Compile()(a) && searchFilter.Compile()(a);
            }

            return filter;
        }

        private System.Linq.Expressions.Expression<Func<Article, bool>> BuildSearchFilter(string search)
        {
            return a => a.Title.Contains(search) || a.Content.Contains(search);
        }

        private ArticleDto MapToDto(Article article)
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
                Era = article.Era != null ? new EraDto
                {
                    Id = article.Era.Id,
                    Name = article.Era.Name,
                    YearStart = article.Era.YearStart,
                    YearEnd = article.Era.YearEnd,
                    Description = article.Era.Description
                } : null,
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
