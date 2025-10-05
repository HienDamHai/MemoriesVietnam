using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesVietnam.Models.Entities
{
    public class OAuthAccount
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string Provider { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string ProviderUserId { get; set; } = string.Empty;

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? ExpireAt { get; set; }

        [Required]
        [StringLength(100)]
        public string LoginId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("LoginId")]
        public virtual Login Login { get; set; } = null!;
    }
}
