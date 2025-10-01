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
    public class CategoryService 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Category>().GetAllAsync();
        }

        // Get only active categories
        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        // Get by id
        public async Task<Category?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Category>().GetByIdAsync(id);
        }

        // Create
        public async Task<Category> CreateAsync(Category category)
        {
            await _unitOfWork.Repository<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return category;
        }

        // Update
        public async Task<Category?> UpdateAsync(string id, string name)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDeleted) return null;

            category.Name = name;

            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();
            return category;
        }

        // Delete (soft delete)
        public async Task<bool> DeleteAsync(string id)
        {
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            if (category == null || category.IsDeleted) return false;

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Category>().Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
