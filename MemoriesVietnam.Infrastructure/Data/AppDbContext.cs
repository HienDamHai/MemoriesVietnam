using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace MemoriesVietnam.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        public DbSet<LikeTable> LikeTables { get; set; }
        public DbSet<HistoryLog> HistoryLogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(MemoriesVietnam.Domain.Common.ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var param = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(param, nameof(ISoftDeletable.IsDeleted));
                    var body = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(body, param);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            // Enum mapping
            modelBuilder.Entity<Login>().Property(x => x.Role).HasConversion<string>();
            modelBuilder.Entity<Order>().Property(x => x.Status).HasConversion<string>();
            modelBuilder.Entity<LikeTable>().Property(x => x.TargetType).HasConversion<string>();
            modelBuilder.Entity<HistoryLog>().Property(x => x.TargetType).HasConversion<string>();
            modelBuilder.Entity<Comment>().Property(x => x.TargetType).HasConversion<string>();
            modelBuilder.Entity<Bookmark>().Property(x => x.TargetType).HasConversion<string>();
            modelBuilder.Entity<Notification>().Property(x => x.TargetType).HasConversion<string>();

            // Self-referencing Comment
            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Replies)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Login)
                .WithMany(l => l.Users)
                .HasForeignKey(u => u.LoginId);

            modelBuilder.Entity<OAuthAccount>()
                .HasOne(o => o.Login)
                .WithMany(l => l.OAuthAccounts)
                .HasForeignKey(o => o.LoginId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Era)
                .WithMany(e => e.Articles)
                .HasForeignKey(a => a.EraId);

            modelBuilder.Entity<ArticleAudio>()
                .HasOne(aa => aa.Article)
                .WithMany(a => a.ArticleAudios)
                .HasForeignKey(aa => aa.ArticleId);

            modelBuilder.Entity<ArticleAudio>()
                .HasOne(aa => aa.CreatedByUser)
                .WithMany(u => u.ArticleAudios)
                .HasForeignKey(aa => aa.CreatedBy);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTags)
                .HasForeignKey(at => at.ArticleId);

            modelBuilder.Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.ArticleTags)
                .HasForeignKey(at => at.TagId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            modelBuilder.Entity<PodcastEpisode>()
                .HasOne(pe => pe.Podcast)
                .WithMany(p => p.Episodes)
                .HasForeignKey(pe => pe.PodcastId);

            modelBuilder.Entity<PodcastEpisode>()
                .HasOne(pe => pe.Article)
                .WithMany()
                .HasForeignKey(pe => pe.ArticleId);
        }

        public override int SaveChanges()
        {
            HandleSoftDelete();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDelete()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted &&
                            e.Entity is MemoriesVietnam.Domain.Common.ISoftDeletable);

            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                ((ISoftDeletable)entry.Entity).IsDeleted = true;
                ((ISoftDeletable)entry.Entity).DeletedAt = DateTime.UtcNow;
            }
        }
    }
}
