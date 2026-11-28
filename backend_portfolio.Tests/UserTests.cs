using System.Net;
using System.Net.Http.Json;
using functions.UserManager.LoginUser;
using functions.UserManager.Me;
using functions.UserManager.RegisterUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models.user;
using TestAPI.Tests;

namespace backend_portfolio.Tests;

[Collection("Api")]
public class UserTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    [Fact]
    public async Task Register_gebruiker_gast_200()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "Test123!";

        //act
        var response = await client.PostAsJsonAsync("UserManager/register", new { email, password });

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(email);

        Assert.NotNull(user);
        Assert.True(await userManager.IsInRoleAsync(user, "Gast"));
        var ts = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();
        Assert.Equal(ts.Email, email);

    }

    [Fact]
    public async Task ongeldig_register_geeft_400()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "123";

        //Act
        var response = await client.PostAsJsonAsync("/UserManager/Register", new { email, password });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_juist_Geeft_Cookie_200()
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
        var me = await client.GetFromJsonAsync<GetUserResponse>("UserManager/me");
        Assert.Equal(email, me.Name);
    }

    [Fact]
    public async Task Login_fout_Wachtwoord_401()
    {
        //Arrange
        var client = factory.CreateClient();
        var email = $"{Guid.NewGuid()}@test.nl";
        var password = "Test123!";
        var wrongPassword = "FoutWachtwoord1!";
        await client.PostAsJsonAsync("/UserManager/register", new { email, password });

        // Act
        var response = await client.PostAsJsonAsync("UserManager/login", new { email, password = wrongPassword });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
    public async Task Me_met_login_geeft_200()
    {
        //arrange
        var client = await factory.MaakClientAsync("Gast");

        // Act
        var response = await client.GetAsync("/UserManager/me");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    

    [Fact]
    public async Task Me_zonder_login_geeft_401()
    {
        //arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/UserManager/me");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logout_geeft_200()
    {
        //arrange
        var client = await factory.MaakClientAsync("Gast");


        //Act
        var logout = await client.PostAsJsonAsync("UserManager/logout", client);
        var response = await client.GetAsync("UserManager/me");

        //Assert
        Assert.Equal(HttpStatusCode.OK, logout.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    }

    [Fact]
    public async Task Logout_zonder_login_401()
    {
        //arrange
        var client = await factory.MaakClientAsync();


        //Act
        var response = await client.PostAsJsonAsync("UserManager/logout", client);
        

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    [Fact]
public async Task Rolwijziging_logt_gebruiker_uit_401()
{
    //Arrange
    var client = await factory.MaakClientAsync("Gast");
    var UserVoorWijziging = await client.GetFromJsonAsync<GetUserResponse>("UserManager/me");
    
    //Act
        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(UserVoorWijziging.Name);
        await userManager.AddToRoleAsync(user!, "Editor");
    
    var naWijziging = await client.GetAsync("UserManager/me");

    //Assert
    Assert.Equal(HttpStatusCode.Unauthorized, naWijziging.StatusCode);
}

}