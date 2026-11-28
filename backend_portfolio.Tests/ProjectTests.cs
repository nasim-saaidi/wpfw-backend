using System.Net;
using System.Net.Http.Json;
using TestAPI.Tests;
using functions.CreateProject;
using functions.GetProject;

namespace backend_portfolio.Tests;


public static class ProjectTestData
{
    public static CreateProjectRequest NieuwProject(string title) => new()
    {
        Title = title,
        Description = "Beschrijving",
        Category = "Web",
        Date = new DateOnly(2026, 1, 1),
        Repo_link = "https://github.com/test/repo",
        Img_alt = "Alt tekst",
        Img_src = "https://example.com/img.png"
    };
}

[Collection("Api")]
public class ProjectsTests(ApiFactory factory) : IClassFixture<ApiFactory>{

    private async Task<GetProjectResponse> MaakProject(string titel)
    {
        var user = await factory.MaakClientAsync("Editor");
        var response = await user.PostAsJsonAsync("projects", ProjectTestData.NieuwProject(titel));
        return (await response.Content.ReadFromJsonAsync<GetProjectResponse>())!;
    }
    //DE README EN INLEVERFILE
    //EN CHECK BIJ DE RUBRIC OF HET DAN KLOPT
    

    [Fact]
    public async Task GetAll_zonder_login_geeft_projects_200()
    {
        //Arrange
        var project = await MaakProject("project voor getall test");
        var client = await factory.MaakClientAsync();

        //Act
        var response = await client.GetAsync("/projects");

        //assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var projecten = await response.Content.ReadFromJsonAsync<List<GetProjectResponse>>();
        Assert.Contains(projecten!, p => p.id == project.id);
    }

    [Fact]
    public async Task GetBy_onbekendid_404()
    {
        //Arrange
        var client = await factory.MaakClientAsync();

        //Act
        var response = await client.GetAsync("projects/100");

        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetBy_successfulId_200()
    {
        //Arrange
        var client = await factory.MaakClientAsync();
        var project = await MaakProject("successvol get");

        //Act
        var response = await client.GetAsync($"projects/{project.id}");

        //assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var target = await client.GetFromJsonAsync<CreateProjectResponse>($"projects/{project.id}");
        Assert.Equal(project.Title, target.Title);
    }

    [Fact]
    public async Task Post_zonder_login_401()
    {
        //Arrange
        var client = await factory.MaakClientAsync();

        //Assert
        var response = await client.PostAsJsonAsync("projects", ProjectTestData.NieuwProject("testtitel"));

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_als_gast_403()
    {
        //Arrange
        var client = await factory.MaakClientAsync("Gast");

        //Act
        var response = await client.PostAsJsonAsync("projects", ProjectTestData.NieuwProject("testtitel"));

        //Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Post_met_goede_rol_201(string role)
    {
        //Arrange
        var client = await factory.MaakClientAsync(role);

        //Act
        var response = await client.PostAsJsonAsync("projects", ProjectTestData.NieuwProject("testtitel"));

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var project = await response.Content.ReadFromJsonAsync<GetProjectResponse>();
        Assert.NotNull(project);
        var target = await client.GetFromJsonAsync<CreateProjectResponse>($"projects/{project.id}");
        Assert.Equal(project.Title, target.Title);
    }

    [Fact]
    public async Task Post_ongeldig_400()
    {
        //Arrange
        var client = await factory.MaakClientAsync("Editor");

        //Act
                var response = await client.PostAsJsonAsync("projects", ProjectTestData.NieuwProject(null));

        //Assert     
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    }

    [Fact]
    public async Task update_zonder_login_401()
    {
        //Arrange
        var client = await factory.MaakClientAsync();
        var project = await MaakProject("testproject");

        //Act
        var response = await client.PutAsJsonAsync($"projects/{project.id}", ProjectTestData.NieuwProject("test voor niet login"));

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    } 

    [Fact]
    public async Task update_als_gast_403()
    {
        //Arrange
        var client = await factory.MaakClientAsync("Gast");
        var project = await MaakProject("testproject");

        //Act
        var response = await client.PutAsJsonAsync($"projects/{project.id}", ProjectTestData.NieuwProject("test voor gast"));

        //Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task update_ongeldig_400()
    {
        //Arrange
        var client = await factory.MaakClientAsync("Editor");
        var project = await MaakProject("testproject");

        //Act
        var response = await client.PutAsJsonAsync($"projects/{project.id}", ProjectTestData.NieuwProject(null));

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

     [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task update_met_goede_rol_204(string rol)
    {
        // Arrange
        var project = await MaakProject("Oude titel");
        var client = await factory.MaakClientAsync(rol);

        // Act
        var response = await client.PutAsJsonAsync($"projects/{project.id}", ProjectTestData.NieuwProject("test titel voor update"));

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var target = await client.GetFromJsonAsync<CreateProjectResponse>($"projects/{project.id}");
        Assert.Equal("test titel voor update", target.Title);
    }

    [Fact]
    public async Task update_OnbekendId_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");

        // Act
        var response = await client.PutAsJsonAsync("/projects/999999", ProjectTestData.NieuwProject("Bestaat niet"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Zonder_Login_401()
    {
        // Arrange
        var project = await MaakProject("delete test Project");
        var client = await factory.MaakClientAsync();

        // Act
        var response = await client.DeleteAsync($"/projects/{project.id}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


     [Fact]
    public async Task Delete_Gast_403()
    {
        // Arrange
        var project = await MaakProject("delete test Project");
        var client = await factory.MaakClientAsync("Gast");

        // Act
        var response = await client.DeleteAsync($"/projects/{project.id}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Delete_met_goede_rol_204(string rol)
    {
        // Arrange
        var project = await MaakProject("test verwijder project");
        var client = await factory.MaakClientAsync(rol);

        // Act
        var response = await client.DeleteAsync($"/projects/{project.id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var target = await client.GetAsync($"/projects/{project.id}");
        Assert.Equal(HttpStatusCode.NotFound, target.StatusCode);
    }

    [Fact]
    public async Task Delete_OnbekendId_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");

        // Act
        var response = await client.DeleteAsync("/projects/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }


    [Fact]
    public async Task Patch_zonder_login_401()
    {
        //Arrange
        var client = await factory.MaakClientAsync();
        var project = await MaakProject("testproject");
        var title = "patch title";

        //Act
        var response = await client.PatchAsJsonAsync($"projects/{project.id}", new {title} );

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

    } 

    [Fact]
    public async Task Patch_als_gast_403()
    {
        //Arrange
        var client = await factory.MaakClientAsync("Gast");
        var project = await MaakProject("testproject");
        var title = "patch title";

        //Act
        var response = await client.PatchAsJsonAsync($"projects/{project.id}", new {title} );

        //Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

    } 



    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Patch_met_goede_rol_204(string rol)
    {
        // Arrange
        var client = await factory.MaakClientAsync(rol);
        var project = await MaakProject("oude titel test project voor patch");
        var title = "patch title";

        // Act
        var response = await client.PatchAsJsonAsync($"projects/{project.id}", new {title} );

        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
       
        var target = await client.GetFromJsonAsync<CreateProjectResponse>($"projects/{project.id}");
        Assert.Equal(title, target.Title);
    }

    [Fact]
    public async Task Patch_OnbekendId_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");
        var title = "patch title";


        // Act
        var response = await client.PatchAsJsonAsync("projects/90909", new {title} );

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Patch_ongeldig_403()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");
        var project = await MaakProject("patch ongeldig");
        var newDate = "hi";
        
        // Act
        var response = await client.PatchAsJsonAsync($"projects/{project.id}", new {date = newDate} );

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


}