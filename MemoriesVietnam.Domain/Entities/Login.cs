using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;
using MemoriesVietnam.Domain.Enum;

namespace MemoriesVietnam.Domain.Entities
{
    public class Login : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public LoginRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Navigation
        public ICollection<User>? Users { get; set; }
        public ICollection<OAuthAccount>? OAuthAccounts { get; set; }
    }
}
