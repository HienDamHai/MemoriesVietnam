using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoriesVietnam.Application.DTOs
{
    public class OAuthAccountDto
    {
        public class OAuthLoginRequest
        {
            public string Provider { get; set; } = "";       // Ví dụ: "Google"
            public string ProviderUserId { get; set; } = ""; // Id từ Google
            public string? Email { get; set; }               // Có thể null nếu provider không cấp
            public string? Name { get; set; }                // Tên từ provider
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
            public DateTime? ExpireAt { get; set; }
        }

        public class OAuthAccountResponseDto
        {
            public string Id { get; set; }
            public string Provider { get; set; }
            public string ProviderUserId { get; set; }
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
            public DateTime? ExpireAt { get; set; }
            public string LoginId { get; set; }
        }
    }
}
