using MemoriesVietnam.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemoriesVietnam.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Login> Logins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OAuthAccount> OAuthAccounts { get; set; }
        public DbSet<Era> Eras { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleAudio> ArticleAudios { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<PodcastEpisode> PodcastEpisodes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<LikeTable> Likes { get; set; }
        public DbSet<HistoryLog> HistoryLogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum conversions
            modelBuilder.Entity<Login>()
                .Property(e => e.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<LikeTable>()
                .Property(e => e.TargetType)
                .HasConversion<string>();

            modelBuilder.Entity<HistoryLog>()
                .Property(e => e.TargetType)
                .HasConversion<string>();

            modelBuilder.Entity<Comment>()
                .Property(e => e.TargetType)
                .HasConversion<string>();

            modelBuilder.Entity<Bookmark>()
                .Property(e => e.TargetType)
                .HasConversion<string>();

            modelBuilder.Entity<Notification>()
                .Property(e => e.TargetType)
                .HasConversion<string>();

            // Configure unique constraints
            modelBuilder.Entity<Login>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Article>()
                .HasIndex(e => e.Slug)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(e => e.Slug)
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<Tag>()
                .HasIndex(e => e.Slug)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Login)
                .WithOne(l => l.User)
                .HasForeignKey<User>(u => u.LoginId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OAuthAccount>()
                .HasOne(o => o.Login)
                .WithMany(l => l.OAuthAccounts)
                .HasForeignKey(o => o.LoginId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Era)
                .WithMany(e => e.Articles)
                .HasForeignKey(a => a.EraId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleAudio>()
                .HasOne(aa => aa.Article)
                .WithMany(a => a.ArticleAudios)
                .HasForeignKey(aa => aa.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArticleAudio>()
                .HasOne(aa => aa.CreatedByUser)
                .WithMany(u => u.ArticleAudios)
                .HasForeignKey(aa => aa.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PodcastEpisode>()
                .HasOne(pe => pe.Podcast)
                .WithMany(p => p.Episodes)
                .HasForeignKey(pe => pe.PodcastId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PodcastEpisode>()
                .HasOne(pe => pe.Article)
                .WithMany(a => a.PodcastEpisodes)
                .HasForeignKey(pe => pe.ArticleId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LikeTable>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HistoryLog>()
                .HasOne(hl => hl.User)
                .WithMany(u => u.HistoryLogs)
                .HasForeignKey(hl => hl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure composite unique constraints
            modelBuilder.Entity<LikeTable>()
                .HasIndex(l => new { l.UserId, l.TargetId, l.TargetType })
                .IsUnique();

            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.TargetId, b.TargetType })
                .IsUnique();

            modelBuilder.Entity<ArticleTag>()
                .HasIndex(at => new { at.ArticleId, at.TagId })
                .IsUnique();
        }
    }
}
