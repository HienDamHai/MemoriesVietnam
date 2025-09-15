using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Common;

namespace MemoriesVietnam.Domain.Entities
{
    public class OAuthAccount : ISoftDeletable
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Provider { get; set; } = "";
        public string ProviderUserId { get; set; } = "";
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpireAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public string LoginId { get; set; }
        public Login Login { get; set; }
    }
}
