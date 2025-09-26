using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
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
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var list = await _unitOfWork.Repository<Product>().GetAllAsync();
            list = list.Where(p => !p.IsDeleted);
            return list;
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var item = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if(item == null || item.IsDeleted) return null;
            return item;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _unitOfWork.Repository<Product>().GetByIdAsync(product.Id);
            if(existing == null) return null;

            existing.Name = product.Name;
            existing.Slug = product.Slug;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.Images = product.Images;
            existing.CategoryId = product.CategoryId;

            _unitOfWork.Repository<Product>().Update(existing);
            await _unitOfWork.SaveChangesAsync();
            return existing;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (existing == null) return false;
            _unitOfWork.Repository<Product>().Remove(existing);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
