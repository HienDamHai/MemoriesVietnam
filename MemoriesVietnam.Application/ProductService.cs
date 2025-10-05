using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Repositories.Interfaces;
using MemoriesVietnam.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MemoriesVietnam.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ✅ Lấy danh sách sản phẩm có phân trang, lọc theo category hoặc search
        public async Task<PaginatedResponse<ProductDto>> GetAllAsync(ProductQuery query)
        {
            var filter = BuildFilter(query);

            var (products, total) = await _unitOfWork.Products.GetPagedAsync(
                query.Page,
                query.Limit,
                filter,
                orderBy: q => q.OrderByDescending(p => p.Id),
                includeProperties: "Category"
            );

            var productDtos = products.Select(MapToDto).ToList();

            return new PaginatedResponse<ProductDto>
            {
                Data = productDtos,
                Total = total,
                Page = query.Page,
                Limit = query.Limit,
                TotalPages = (int)Math.Ceiling((double)total / query.Limit)
            };
        }

        // ✅ Lấy sản phẩm theo Id
        public async Task<ProductDto?> GetByIdAsync(string id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        // ✅ Lấy sản phẩm theo Slug
        public async Task<ProductDto?> GetBySlugAsync(string slug)
        {
            var product = await _unitOfWork.Products.FindFirstAsync(p => p.Slug == slug, includeProperties: "Category");
            return product == null ? null : MapToDto(product);
        }

        // ✅ Lấy danh sách sản phẩm nổi bật (ví dụ lấy 6 sản phẩm đầu)
        public async Task<List<ProductDto>> GetFeaturedAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync(includeProperties: "Category");
            return products.Take(6).Select(MapToDto).ToList();
        }

        // ✅ Tạo sản phẩm mới
        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Slug = GenerateSlug(dto.Name),
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageArray = dto.Images,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(product);
        }

        // ✅ Cập nhật sản phẩm
        public async Task<ProductDto?> UpdateAsync(string id, UpdateProductDto dto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;

            if (!string.IsNullOrEmpty(dto.Name))
            {
                product.Name = dto.Name;
                product.Slug = GenerateSlug(dto.Name);
            }

            if (!string.IsNullOrEmpty(dto.Description))
                product.Description = dto.Description;

            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;

            if (dto.Stock.HasValue)
                product.Stock = dto.Stock.Value;

            if (dto.Images != null)
                product.ImageArray = dto.Images;

            if (!string.IsNullOrEmpty(dto.CategoryId))
                product.CategoryId = dto.CategoryId;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(product);
        }

        // ✅ Xóa sản phẩm
        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            await _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // ✅ Lấy danh sách danh mục
        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        // ✅ Tạo danh mục mới
        public async Task<CategoryDto> CreateCategoryAsync(string name)
        {
            var category = new Category
            {
                Name = name
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        // ✅ Sinh slug từ tên
        public string GenerateSlug(string name)
        {
            var slug = name.ToLowerInvariant();
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');
            return slug;
        }

        // ======= Helper Methods =======
        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Images = product.ImageArray,
                CategoryId = product.CategoryId,
                Category = product.Category != null
                    ? new CategoryDto
                    {
                        Id = product.Category.Id,
                        Name = product.Category.Name
                    }
                    : null
            };
        }

        private System.Linq.Expressions.Expression<Func<Product, bool>>? BuildFilter(ProductQuery query)
        {
            System.Linq.Expressions.Expression<Func<Product, bool>>? filter = null;

            if (!string.IsNullOrEmpty(query.CategoryId))
                filter = p => p.CategoryId == query.CategoryId;

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.ToLower();
                var searchFilter = (System.Linq.Expressions.Expression<Func<Product, bool>>)(p =>
                    p.Name.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search))
                );

                if (filter != null)
                {
                    var compiled = filter.Compile();
                    var searchCompiled = searchFilter.Compile();
                    filter = p => compiled(p) && searchCompiled(p);
                }
                else
                {
                    filter = searchFilter;
                }
            }

            return filter;
        }
    }
}
