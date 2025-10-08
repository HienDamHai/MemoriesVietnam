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
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _repo;
        public ProductService(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _repo = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            list = list.Where(p => !p.IsDeleted);
            return list;
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var item = await _repo.GetByIdAsync(id);
            if(item == null || item.IsDeleted) return null;
            return item;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _repo.GetByIdAsync(product.Id);
            if(existing == null) return null;

            existing.Name = product.Name;
            existing.Slug = product.Slug;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.Images = product.Images;
            existing.CategoryId = product.CategoryId;

            _repo.Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _repo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;
            _repo.Remove(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
