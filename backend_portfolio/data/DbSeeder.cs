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

        foreach(var role in new[] {"Admin", "Editor", "gast"})
        {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));

        }}

        // var adminUser = await userManager.FindByEmailAsync("admin@wpfw.com");
        // if (adminUser is null)
        // {
        //     adminUser = new User
        //     {
        //         UserName = "admin@wpfw.com",
        //         Email = "admin@wpfw.com",
        //         EmailConfirmed = true
        //     };
        //     await userManager.CreateAsync(
        //         adminUser,
        //         "Admin123!"
        //     );
        // }

        // if(!await userManager.IsInRoleAsync(adminUser, adminRole))
        // {
        //     await userManager.AddToRoleAsync(
        //         adminUser,
        //         adminRole
        //     );
        // }

    }
}