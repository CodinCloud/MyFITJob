using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyFITJob.Identity.Data;

namespace MyFITJob.Identity.Seed;

public static class RolesSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        var roles = new[] { "Student", "Recruiter", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var addingRole = new ApplicationRole() { Name = role, };
                var result = await roleManager.CreateAsync(addingRole);
                if (result.Succeeded)
                {
                    logger.LogInformation("Role {Role} created successfully", role);
                }
                else
                {
                    logger.LogError("Failed to create role {Role}: {Errors}", role, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        var adminEmail = "admin@myfitjob.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(adminUser, "admin123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                logger.LogInformation("Default admin user created successfully with email: {Email}", adminEmail);
            }
            else
            {
                logger.LogError("Failed to create default admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            logger.LogInformation("Admin user already exists with email: {Email}", adminEmail);
        }
    }
} 