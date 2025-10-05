using MemoriesVietnam.Models.DTOs;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IArticleService
    {
        Task<PaginatedResponse<ArticleDto>> GetAllAsync(ArticleQuery query);
        Task<ArticleDto?> GetByIdAsync(string id);
        Task<ArticleDto?> GetBySlugAsync(string slug);
        Task<List<ArticleDto>> GetFeaturedAsync();
        Task<PaginatedResponse<ArticleDto>> SearchAsync(string query, int page, int limit);
        Task<ArticleDto> CreateAsync(CreateArticleDto dto);
        Task<ArticleDto?> UpdateAsync(string id, UpdateArticleDto dto);
        Task<bool> DeleteAsync(string id);
        Task<PaginatedResponse<ArticleDto>> GetByEraAsync(string eraId, int page, int limit);
        string GenerateSlug(string title);
    }
}
