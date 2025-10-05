using MemoriesVietnam.Models.DTOs;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IProductService
    {
        Task<PaginatedResponse<ProductDto>> GetAllAsync(ProductQuery query);
        Task<ProductDto?> GetByIdAsync(string id);
        Task<ProductDto?> GetBySlugAsync(string slug);
        Task<List<ProductDto>> GetFeaturedAsync();
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto?> UpdateAsync(string id, UpdateProductDto dto);
        Task<bool> DeleteAsync(string id);
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(string name);
        string GenerateSlug(string name);
    }
}
