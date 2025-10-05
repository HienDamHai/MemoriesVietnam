using MemoriesVietnam.Models.DTOs;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IEraService
    {
        Task<List<EraDto>> GetAllAsync();
        Task<EraDto?> GetByIdAsync(string id);
        Task<EraDto> CreateAsync(CreateEraDto dto);
        Task<EraDto?> UpdateAsync(string id, UpdateEraDto dto);
        Task<bool> DeleteAsync(string id);
        Task<PaginatedResponse<ArticleDto>> GetArticlesByEraAsync(string eraId, int page, int limit);
    }
}
