using ApplicationLayer.Interfaces.Infrastructure;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DbContext.DataSeed
{
    public class DbSeeder(UserManager<User> userManager) : IDbSeeder
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task SeedAsync()
        {
            const string adminEmail = "admin@possystem.com";
            const string adminPassword = "Admin123#$";

            var user = await _userManager.FindByEmailAsync(adminEmail);
            if (user == null)
            {
                var newUser = new User
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "POS System",
                    LastName = "Administrator",
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, adminPassword);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Error al crear usuario admin: {errors}")!;
                }
            }
        }

    }
}
