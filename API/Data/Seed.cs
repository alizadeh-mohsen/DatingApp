using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var jsonUserData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var serilizeOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var users = JsonSerializer.Deserialize<List<AppUser>>(jsonUserData, serilizeOptions);

            var roles = new List<AppRole>
            {
                new AppRole{Name="Admin"},
                new AppRole{Name="Member"},
                new AppRole{Name="Moderator"},
            };

            foreach (AppRole role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
                user.LastActive = DateTime.SpecifyKind(user.LastActive, DateTimeKind.Utc);
                await userManager.CreateAsync(user, "1");
                if (user.UserName == "lisa")
                    await userManager.AddToRoleAsync(user, "Admin");
                else
                    await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin",
            };

            await userManager.CreateAsync(admin, "1");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

        }
    }
}
