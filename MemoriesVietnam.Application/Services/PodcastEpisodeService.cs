using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.Services
{
    public class PodcastEpisodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PodcastEpisodeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PodcastEpisode>> GetAllAsync()
        {
            return await _unitOfWork.Repository<PodcastEpisode>().GetAllAsync();
        }
        public async Task<PodcastEpisode?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<PodcastEpisode>().GetByIdAsync(id);
        }
        public async Task<PodcastEpisode> CreateAsync(PodcastEpisode podcastEpisode)
        {
            await _unitOfWork.Repository<PodcastEpisode>().AddAsync(podcastEpisode);
            await _unitOfWork.SaveChangesAsync();
            return podcastEpisode;
        }
        public async Task<PodcastEpisode?> UpdateAsync(PodcastEpisode podcastEpisode)
        {
            var existingPodcastEpisode = await _unitOfWork.Repository<PodcastEpisode>().GetByIdAsync(podcastEpisode.Id);
            if (existingPodcastEpisode == null || existingPodcastEpisode.IsDeleted)
                return null;
            existingPodcastEpisode.Title = podcastEpisode.Title;
            existingPodcastEpisode.AudioUrl = podcastEpisode.AudioUrl;
            existingPodcastEpisode.Duration = podcastEpisode.Duration;
            existingPodcastEpisode.EpisodeNumber = podcastEpisode.EpisodeNumber;
            existingPodcastEpisode.ArticleId = podcastEpisode.ArticleId;

            _unitOfWork.Repository<PodcastEpisode>().Update(existingPodcastEpisode);
            await _unitOfWork.SaveChangesAsync();
            return existingPodcastEpisode;
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var existingPodcastEpisode = await _unitOfWork.Repository<PodcastEpisode>().GetByIdAsync(id);
            if (existingPodcastEpisode == null || existingPodcastEpisode.IsDeleted)
                return false;

            _unitOfWork.Repository<PodcastEpisode>().Remove(existingPodcastEpisode);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
