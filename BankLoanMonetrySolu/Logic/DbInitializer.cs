using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace BankLoanMonetrySolu.Logic;
public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        // Define your required roles and their abbreviations/claims representation
        string[] roleNames = { "CMO", "CMS", "AO" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    logger.LogInformation($"Successfully seeded role: {roleName}");
                }
                else
                {
                    logger.LogError($"Failed to seed role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}