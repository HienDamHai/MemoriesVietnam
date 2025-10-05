using MemoriesVietnam.Models.DTOs;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Repositories.Interfaces;
using MemoriesVietnam.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MemoriesVietnam.Services
{
    public class PodcastService : IPodcastService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PodcastService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 📌 Lấy tất cả podcast (bao gồm Episodes)
        public async Task<List<PodcastDto>> GetAllAsync()
        {
            var podcasts = await _unitOfWork.Podcasts.GetAllAsync();
            var result = new List<PodcastDto>();

            foreach (var podcast in podcasts)
            {
                var episodes = await _unitOfWork.PodcastEpisodes
                    .FindAsync(e => e.PodcastId == podcast.Id);

                result.Add(MapToDto(podcast, episodes));
            }

            return result.OrderByDescending(p => p.CreatedAt).ToList();
        }

        // 📌 Lấy podcast theo ID (bao gồm Episodes)
        public async Task<PodcastDto?> GetByIdAsync(string id)
        {
            var podcast = await _unitOfWork.Podcasts.GetByIdAsync(id);
            if (podcast == null) return null;

            var episodes = await _unitOfWork.PodcastEpisodes
                .FindAsync(e => e.PodcastId == id);

            return MapToDto(podcast, episodes);
        }

        // 📌 Tạo mới Podcast
        public async Task<PodcastDto> CreateAsync(CreatePodcastDto dto)
        {
            var podcast = new Podcast
            {
                Title = dto.Title,
                Description = dto.Description,
                CoverUrl = dto.CoverUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Podcasts.AddAsync(podcast);
            await _unitOfWork.SaveChangesAsync();

            return MapToDto(podcast);
        }

        // 📌 Cập nhật Podcast
        public async Task<PodcastDto?> UpdateAsync(string id, CreatePodcastDto dto)
        {
            var podcast = await _unitOfWork.Podcasts.GetByIdAsync(id);
            if (podcast == null) return null;

            podcast.Title = dto.Title;
            podcast.Description = dto.Description;
            podcast.CoverUrl = dto.CoverUrl;
            podcast.CreatedAt = podcast.CreatedAt; // giữ nguyên
            podcast = _unitOfWork.Podcasts.UpdateEntity(podcast);

            await _unitOfWork.SaveChangesAsync();
            return MapToDto(podcast);
        }

        // 📌 Xóa Podcast (và các Episode liên quan)
        public async Task<bool> DeleteAsync(string id)
        {
            var podcast = await _unitOfWork.Podcasts.GetByIdAsync(id);
            if (podcast == null) return false;

            var episodes = await _unitOfWork.PodcastEpisodes.FindAsync(e => e.PodcastId == id);
            if (episodes.Any())
                await _unitOfWork.PodcastEpisodes.DeleteRangeAsync(episodes);

            await _unitOfWork.Podcasts.DeleteAsync(podcast);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // 📌 Lấy tất cả tập theo PodcastId
        public async Task<List<PodcastEpisodeDto>> GetEpisodesByPodcastAsync(string podcastId)
        {
            var episodes = await _unitOfWork.PodcastEpisodes
                .FindAsync(e => e.PodcastId == podcastId);

            var articleIds = episodes.Where(e => e.ArticleId != null)
                                     .Select(e => e.ArticleId!)
                                     .ToList();

            // include Article nếu có
            var articles = new Dictionary<string, Article>();
            if (articleIds.Any())
            {
                var articleList = await _unitOfWork.Articles.FindAsync(a => articleIds.Contains(a.Id));
                articles = articleList.ToDictionary(a => a.Id, a => a);
            }

            return episodes.OrderBy(e => e.EpisodeNumber)
                           .Select(e => MapToEpisodeDto(e, articles.GetValueOrDefault(e.ArticleId ?? "")))
                           .ToList();
        }

        // 📌 Tạo mới tập Podcast
        public async Task<PodcastEpisodeDto> CreateEpisodeAsync(string podcastId, CreatePodcastEpisodeDto dto)
        {
            var podcast = await _unitOfWork.Podcasts.GetByIdAsync(podcastId);
            if (podcast == null)
                throw new Exception("Podcast không tồn tại");

            var episode = new PodcastEpisode
            {
                PodcastId = podcastId,
                Title = dto.Title,
                AudioUrl = dto.AudioUrl,
                Duration = dto.Duration,
                ArticleId = dto.ArticleId,
                EpisodeNumber = dto.EpisodeNumber
            };

            await _unitOfWork.PodcastEpisodes.AddAsync(episode);
            await _unitOfWork.SaveChangesAsync();

            Article? article = null;
            if (!string.IsNullOrEmpty(dto.ArticleId))
                article = await _unitOfWork.Articles.FindFirstAsync(a => a.Id == dto.ArticleId);

            return MapToEpisodeDto(episode, article);
        }

        // 📌 Lấy danh sách tập mới nhất
        public async Task<List<PodcastEpisodeDto>> GetLatestEpisodesAsync()
        {
            var (episodes, _) = await _unitOfWork.PodcastEpisodes.GetPagedAsync(
                1, 10,
                orderBy: q => q.OrderByDescending(e => e.EpisodeNumber),
                includeProperties: "Podcast,Article"
            );

            return episodes.Select(e => MapToEpisodeDto(e, e.Article))
                           .ToList();
        }

        // ----------------- MAPPING -----------------

        private PodcastDto MapToDto(Podcast podcast, IEnumerable<PodcastEpisode>? episodes = null)
        {
            return new PodcastDto
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description,
                CoverUrl = podcast.CoverUrl,
                CreatedAt = podcast.CreatedAt,
                Episodes = episodes?.Select(e => new PodcastEpisodeDto
                {
                    Id = e.Id,
                    PodcastId = e.PodcastId,
                    Title = e.Title,
                    AudioUrl = e.AudioUrl,
                    Duration = e.Duration,
                    ArticleId = e.ArticleId,
                    EpisodeNumber = e.EpisodeNumber
                }).ToList() ?? new List<PodcastEpisodeDto>()
            };
        }

        private PodcastEpisodeDto MapToEpisodeDto(PodcastEpisode episode, Article? article = null)
        {
            return new PodcastEpisodeDto
            {
                Id = episode.Id,
                PodcastId = episode.PodcastId,
                Title = episode.Title,
                AudioUrl = episode.AudioUrl,
                Duration = episode.Duration,
                ArticleId = episode.ArticleId,
                EpisodeNumber = episode.EpisodeNumber,
                Article = article != null ? new ArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    CoverUrl = article.CoverUrl,
                    YearStart = article.YearStart,
                    YearEnd = article.YearEnd,
                    EraId = article.EraId
                } : null
            };
        }
    }

    // 🔧 Extension nhỏ cho tiện: cập nhật entity mà không async
    public static class RepositoryExtensions
    {
        public static T UpdateEntity<T>(this IGenericRepository<T> repo, T entity) where T : class
        {
            repo.UpdateAsync(entity);
            return entity;
        }
    }
}
