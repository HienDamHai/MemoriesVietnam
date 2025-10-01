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
    public class ArticleAudioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleAudioRepository _articleAudioRepository;

        public ArticleAudioService(IUnitOfWork unitOfWork, IArticleAudioRepository articleAudioRepository)
        {
            _unitOfWork = unitOfWork;
            _articleAudioRepository = articleAudioRepository;
        }

        public async Task<IEnumerable<ArticleAudio>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ArticleAudio>().GetAllAsync();
        }

        public async Task<ArticleAudio?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<ArticleAudio>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<ArticleAudio>> GetByArticleIdAsync(string articleId)
        {
            return await _articleAudioRepository.GetByArticleIdAsync(articleId);
        }

        public async Task<IEnumerable<ArticleAudio>> GetByUserIdAsync(string userId)
        {
            return await _articleAudioRepository.GetByUserIdAsync(userId);
        }

        public async Task<ArticleAudio> CreateAsync(ArticleAudio audio)
        {
            await _unitOfWork.Repository<ArticleAudio>().AddAsync(audio);
            await _unitOfWork.SaveChangesAsync();
            return audio;
        }

        public async Task<ArticleAudio?> UpdateAsync(string id, string voiceId, string url, int duration)
        {
            var audio = await _unitOfWork.Repository<ArticleAudio>().GetByIdAsync(id);
            if (audio == null || audio.IsDeleted) return null;

            audio.VoiceId = voiceId;
            audio.Url = url;
            audio.Duration = duration;

            _unitOfWork.Repository<ArticleAudio>().Update(audio);
            await _unitOfWork.SaveChangesAsync();

            return audio;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var audio = await _unitOfWork.Repository<ArticleAudio>().GetByIdAsync(id);
            if (audio == null || audio.IsDeleted) return false;

            audio.IsDeleted = true;
            audio.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ArticleAudio>().Update(audio);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
