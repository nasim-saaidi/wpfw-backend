using main.db;
using Microsoft.AspNetCore.Identity;
using Models.user;

namespace data.DbSeeder;

public static class DbSeeder
{
public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();

        var context = scope.ServiceProvider
        .GetRequiredService<PortDB>();

        var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<User>>();

        var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

        // const string adminRole = "Admin";

        foreach(var role in new[] {"Admin", "Editor", "Gast"})
        {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));

        }}

    

    }
}