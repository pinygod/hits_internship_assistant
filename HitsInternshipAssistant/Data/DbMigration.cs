using HitsInternshipAssistant.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HitsInternshipAssistant.Data
{
    public static class DbMigration
    {
        public static IHost MigrateDbContext<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                DbMigration.ConfigureIdentity(scope).GetAwaiter().GetResult();
            }

            return host;
        }

        private static async Task ConfigureIdentity(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var adminsRole = await roleManager.FindByNameAsync(Roles.Admin);
            if (adminsRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {Roles.Admin} role.");
                }

                adminsRole = await roleManager.FindByNameAsync(Roles.Admin);
            }

            var adminUser = await userManager.FindByNameAsync("admin@localhost.local");
            if (adminUser == null)
            {
                var userResult = await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@localhost.local",
                    Email = "admin@localhost.local",
                    FirstName = "Admin",
                    LastName = "Admin"
                }, "AdminPass123!");
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create admin@localhost.local user");
                }

                adminUser = await userManager.FindByNameAsync("admin@localhost.local");
            }

            if (!await userManager.IsInRoleAsync(adminUser, adminsRole.Name))
            {
                await userManager.AddToRoleAsync(adminUser, adminsRole.Name);
            }
        }
    }
}
