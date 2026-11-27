using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models.user;
using TestAPI.Tests;

namespace backend_portfolio.Tests;

[Collection("Api")]
public class UserTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Register_gebruiker_gast()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "Test123!";

        //act
        var response = await client.PostAsJsonAsync("UserManager/register", new {email, password});

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(email);
        
        Assert.NotNull(user);
        Assert.True(await userManager.IsInRoleAsync(user, "Gast"));
    }

    [Fact]
    public async Task zwak_wachtwoord_register_geeft400()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "123";

        //Act
        var response = await client.PostAsJsonAsync("/UserManager/Register", new { email, password});

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task Login_juist_GeeftCookie()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "Test123!";
        await client.PostAsJsonAsync("UserManager/register", new { email, password });


        //Act
        var response = await client.PostAsJsonAsync("/UserManager/login", new { email, password });
    
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.Contains("Set-Cookie"));
    }

    [Fact]
    public async Task Login_foutWachtwoord_Geeft401()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "Test123!";
        var wrongPassword = "FoutWachtwoord1!";
        await client.PostAsJsonAsync("/UserManager/register", new { email, password});

        // Act
        var response = await client.PostAsJsonAsync("UserManager/login", new { email, password = wrongPassword });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_zonderlogin_geeft401()
    {
        //arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/UserManager/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    
}