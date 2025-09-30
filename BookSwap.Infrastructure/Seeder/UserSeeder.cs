using BookSwap.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Infrastructure.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> _userManager)
        {
            var usersCount = await _userManager.Users.CountAsync();
            if (usersCount <= 0)
            {
                var defaultuser = new User()
                {
                    UserName = "moner",
                    Email = "moner@example.com",
                    FirstName = "Moner",
                    LastName = "Nashed",
                    PhoneNumber = "123456",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await _userManager.CreateAsync(defaultuser, "Moner-2003");
                await _userManager.AddToRoleAsync(defaultuser, "Admin");
                await _userManager.AddToRoleAsync(defaultuser, "User");
            }
        }
    }
}
