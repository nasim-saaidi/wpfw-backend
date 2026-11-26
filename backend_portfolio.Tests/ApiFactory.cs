using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using main.db;
using Models.user;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;


namespace TestAPI.Tests;

public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((context, config) =>
{
    config.AddUserSecrets<Program>();
});
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PortDB>();


        db.Database.Migrate();

        return host;
    }

    public async Task<HttpClient> MaakClientAsync(string? rol = null)
    {
        var client = CreateClient();
        if (rol is null)
        {
            return client;
        }
        ;
        var email = $"{Guid.NewGuid()}@test.com";
        var password = "Test123!";

        var userobj = new { email, password };


        await client.PostAsJsonAsync("/UserManager/register", userobj);


        if (rol != "Gast")
        {
            using var scope = Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var user = await userManager.FindByEmailAsync(email);
            await userManager.AddToRoleAsync(user!, rol);
        }





        await client.PostAsJsonAsync("/UserManager/Login", userobj);
        return client;
    }

}

