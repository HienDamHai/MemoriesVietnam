using MemoriesVietnam.Models.Entities;

namespace MemoriesVietnam.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Login> Logins { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<OAuthAccount> OAuthAccounts { get; }
        IGenericRepository<Era> Eras { get; }
        IGenericRepository<Article> Articles { get; }
        IGenericRepository<ArticleAudio> ArticleAudios { get; }
        IGenericRepository<Podcast> Podcasts { get; }
        IGenericRepository<PodcastEpisode> PodcastEpisodes { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<LikeTable> Likes { get; }
        IGenericRepository<HistoryLog> HistoryLogs { get; }
        IGenericRepository<Comment> Comments { get; }
        IGenericRepository<Bookmark> Bookmarks { get; }
        IGenericRepository<Tag> Tags { get; }
        IGenericRepository<ArticleTag> ArticleTags { get; }
        IGenericRepository<Notification> Notifications { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
