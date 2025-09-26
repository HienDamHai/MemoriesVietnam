using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class TagService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Tag>().GetAllAsync();
        }

        public async Task<Tag?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Tag>().GetByIdAsync(id);
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _unitOfWork.Repository<Tag>().AddAsync(tag);
            await _unitOfWork.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _unitOfWork.Repository<Tag>().GetByIdAsync(tag.Id);
            if (existingTag == null || existingTag.IsDeleted) return null;

            existingTag.Name = tag.Name;
            existingTag.Slug = tag.Slug;

            await _unitOfWork.SaveChangesAsync();

            return existingTag;
        }


        public async Task<bool> DeleteAsync(string id)
        {
            var existingTag = await _unitOfWork.Repository<Tag>().GetByIdAsync(id);
            if (existingTag == null || existingTag.IsDeleted) return false;
            _unitOfWork.Repository<Tag>().Remove(existingTag);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
