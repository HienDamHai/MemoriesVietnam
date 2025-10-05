using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoriesVietnam.Models.Entities
{
    public class Login
    {
        [Key]
        [StringLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? PasswordHash { get; set; }

        public UserRole Role { get; set; } = UserRole.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<OAuthAccount> OAuthAccounts { get; set; } = new List<OAuthAccount>();
    }

    public enum UserRole
    {
        Admin,
        Editor,
        User
    }
}
