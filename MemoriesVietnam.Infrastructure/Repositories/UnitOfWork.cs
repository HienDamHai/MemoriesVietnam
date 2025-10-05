using Microsoft.EntityFrameworkCore.Storage;
using MemoriesVietnam.Models.Entities;
using MemoriesVietnam.Repositories.Interfaces;
using MemoriesVietnam.Infrastructure.Data;

namespace MemoriesVietnam.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            Logins = new GenericRepository<Login>(_context);
            Users = new GenericRepository<User>(_context);
            OAuthAccounts = new GenericRepository<OAuthAccount>(_context);
            Eras = new GenericRepository<Era>(_context);
            Articles = new GenericRepository<Article>(_context);
            ArticleAudios = new GenericRepository<ArticleAudio>(_context);
            Podcasts = new GenericRepository<Podcast>(_context);
            PodcastEpisodes = new GenericRepository<PodcastEpisode>(_context);
            Categories = new GenericRepository<Category>(_context);
            Products = new GenericRepository<Product>(_context);
            Orders = new GenericRepository<Order>(_context);
            OrderItems = new GenericRepository<OrderItem>(_context);
            Likes = new GenericRepository<LikeTable>(_context);
            HistoryLogs = new GenericRepository<HistoryLog>(_context);
            Comments = new GenericRepository<Comment>(_context);
            Bookmarks = new GenericRepository<Bookmark>(_context);
            Tags = new GenericRepository<Tag>(_context);
            ArticleTags = new GenericRepository<ArticleTag>(_context);
            Notifications = new GenericRepository<Notification>(_context);
        }

        public IGenericRepository<Login> Logins { get; private set; }
        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<OAuthAccount> OAuthAccounts { get; private set; }
        public IGenericRepository<Era> Eras { get; private set; }
        public IGenericRepository<Article> Articles { get; private set; }
        public IGenericRepository<ArticleAudio> ArticleAudios { get; private set; }
        public IGenericRepository<Podcast> Podcasts { get; private set; }
        public IGenericRepository<PodcastEpisode> PodcastEpisodes { get; private set; }
        public IGenericRepository<Category> Categories { get; private set; }
        public IGenericRepository<Product> Products { get; private set; }
        public IGenericRepository<Order> Orders { get; private set; }
        public IGenericRepository<OrderItem> OrderItems { get; private set; }
        public IGenericRepository<LikeTable> Likes { get; private set; }
        public IGenericRepository<HistoryLog> HistoryLogs { get; private set; }
        public IGenericRepository<Comment> Comments { get; private set; }
        public IGenericRepository<Bookmark> Bookmarks { get; private set; }
        public IGenericRepository<Tag> Tags { get; private set; }
        public IGenericRepository<ArticleTag> ArticleTags { get; private set; }
        public IGenericRepository<Notification> Notifications { get; private set; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
