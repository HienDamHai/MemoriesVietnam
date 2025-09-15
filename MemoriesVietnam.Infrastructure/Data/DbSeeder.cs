using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoriesVietnam.Domain.Entities;
using MemoriesVietnam.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MemoriesVietnam.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmin(AppDbContext context, IConfiguration config)
        {
            context.Database.Migrate();

            var adminEmail = config["AdminAccount:Email"];
            var adminPassword = config["AdminAccount:Password"];
            var adminName = config["AdminAccount:Name"];

            if(!context.Logins.Any(l => l.Email == adminEmail))
            {
                var login = new Login
                {
                    Email = adminEmail,
                    Role = LoginRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword)
                };

                var user = new User
                {
                    Name = adminName,
                    Login = login,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                context.Logins.Add(login);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
