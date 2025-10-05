using MemoriesVietnam.Models.DTOs;

namespace MemoriesVietnam.Services.Interfaces
{
    public interface IPodcastService
    {
        Task<List<PodcastDto>> GetAllAsync();
        Task<PodcastDto?> GetByIdAsync(string id);
        Task<PodcastDto> CreateAsync(CreatePodcastDto dto);
        Task<PodcastDto?> UpdateAsync(string id, CreatePodcastDto dto);
        Task<bool> DeleteAsync(string id);
        Task<List<PodcastEpisodeDto>> GetEpisodesByPodcastAsync(string podcastId);
        Task<PodcastEpisodeDto> CreateEpisodeAsync(string podcastId, CreatePodcastEpisodeDto dto);
        Task<List<PodcastEpisodeDto>> GetLatestEpisodesAsync();
    }
}
