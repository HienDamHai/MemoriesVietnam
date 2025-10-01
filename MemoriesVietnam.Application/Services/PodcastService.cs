using MemoriesVietnam.Application.DTOs;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.IBasic;
using MemoriesVietnam.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoriesVietnam.Application.DTOs.PodcastDto.CreatePodcastDto;

namespace MemoriesVietnam.Application.Services
{
    public class PodcastService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPodcastRepository _podcastRepository;
        public PodcastService(IUnitOfWork unitOfWork, IPodcastRepository podcastRepository)
        {
            _unitOfWork = unitOfWork;
            _podcastRepository = podcastRepository;
        }

        public async Task<IEnumerable<Podcast>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Podcast>().GetAllAsync();
        }

        public async Task<Podcast?> GetByIdAsync(string id)
        {
            return await _unitOfWork.Repository<Podcast>().GetByIdAsync(id);
        }

        public async Task<Podcast> CreateAsync(Podcast podcast)
        {
            await _unitOfWork.Repository<Podcast>().AddAsync(podcast);
            await _unitOfWork.SaveChangesAsync();
            return podcast;
        }

        public async Task<Podcast?> UpdateAsync(Podcast podcast)
        {
            var existingPodcast = await _unitOfWork.Repository<Podcast>().GetByIdAsync(podcast.Id);
            if (existingPodcast == null || existingPodcast.IsDeleted)
                return null;

            existingPodcast.Title = podcast.Title;
            existingPodcast.Description = podcast.Description;
            existingPodcast.CoverUrl = podcast.CoverUrl;

            _unitOfWork.Repository<Podcast>().Update(existingPodcast);
            await _unitOfWork.SaveChangesAsync();
            return existingPodcast;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existingPodcast = await _unitOfWork.Repository<Podcast>().GetByIdAsync(id);
            if (existingPodcast == null || existingPodcast.IsDeleted)
                return false;

            _unitOfWork.Repository<Podcast>().Remove(existingPodcast);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Podcast>> GetAllWithEpisodeAsync()
        {
            var podcasts = await _podcastRepository.GetAllWithEpisodesAsync();

            return podcasts;
        }
    }
}
