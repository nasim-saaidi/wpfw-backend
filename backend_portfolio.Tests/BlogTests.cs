
using System.Net;
using System.Net.Http.Json;
using TestAPI.Tests;
using Models.blog;
using functions.CreateBlog;

namespace backend_portfolio.Tests;

public static class BlogTestData
{
    public static Blog NieuwBlog(string title) => new()
    {
        Title = title,
        Content = "Blog inhoud",
        Date = new DateTime(2026, 1, 1),
        InternalNotes = "Interne notities"
    };
}

[Collection("Api")]
public class BlogsTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private async Task<CreateBlogResponse> MaakBlog(string titel)
    {
        var user = await factory.MaakClientAsync("Editor");
        var response = await user.PostAsJsonAsync("blogs", BlogTestData.NieuwBlog(titel));
        return (await response.Content.ReadFromJsonAsync<CreateBlogResponse>())!;
    }

    [Fact]
    public async Task GetAll_zonder_login_geeft_blogs()
    {
        // Arrange
        var blog = await MaakBlog("blog voor getall test");
        var client = await factory.MaakClientAsync();

        // Act
        var response = await client.GetAsync("/blogs");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var blogs = await response.Content.ReadFromJsonAsync<List<CreateBlogResponse>>();
        Assert.Contains(blogs!, b => b.Id == blog.Id);
    }

    [Fact]
    public async Task GetBlog_geeft_niet_InternalNotes()
    {
        // Arrange
        var blog = await MaakBlog("test blog");
        var client = await factory.MaakClientAsync();

        // Act
        var response = await client.GetAsync($"blogs/{blog.Id}");

        var target = await response.Content.ReadAsStringAsync();

        Assert.DoesNotContain("InternalNotes", target);
        Assert.DoesNotContain("Interne notities", target);
    }

    [Fact]
    public async Task GetBy_onbekendid_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync();

        // Act
        var response = await client.GetAsync("blogs/100");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetBy_successfulId_200()
    {
        // Arrange
        var client = await factory.MaakClientAsync();
        var blog = await MaakBlog("succesvol get");

        // Act
        var response = await client.GetAsync($"blogs/{blog.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var ts = await response.Content.ReadFromJsonAsync<CreateBlogResponse>();
        Assert.NotNull(ts);
        var target = await client.GetFromJsonAsync<CreateBlogResponse>($"blogs/{ts.Id}");
        Assert.Equal(blog.Title, target.Title);
    }

    [Fact]
    public async Task Post_zonder_login_401()
    {
        // Arrange
        var client = await factory.MaakClientAsync();

        // Act
        var response = await client.PostAsJsonAsync("blogs", BlogTestData.NieuwBlog("testtitel"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_als_gast_403()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Gast");

        // Act
        var response = await client.PostAsJsonAsync("blogs", BlogTestData.NieuwBlog("testtitel"));

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Post_met_goede_rol_201(string role)
    {
        // Arrange
        var client = await factory.MaakClientAsync(role);

        // Act
        var response = await client.PostAsJsonAsync("blogs", BlogTestData.NieuwBlog("testtitel"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var blog = await response.Content.ReadFromJsonAsync<CreateBlogResponse>();
        Assert.NotNull(blog);
        var target = await client.GetFromJsonAsync<CreateBlogResponse>($"blogs/{blog.Id}");
        Assert.Equal(blog.Title, target.Title);
    }

    [Fact]
    public async Task Post_ongeldig_400()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");

        // Act
        var response = await client.PostAsJsonAsync("blogs", BlogTestData.NieuwBlog(null));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task update_zonder_login_401()
    {
        // Arrange
        var client = factory.CreateClient();
        var blog = await MaakBlog("testblog");

        // Act
        var response = await client.PutAsJsonAsync(
            $"blogs/{blog.Id}",
            BlogTestData.NieuwBlog("test voor niet login"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task update_als_gast_403()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Gast");
        var blog = await MaakBlog("testblog");

        // Act
        var response = await client.PutAsJsonAsync(
            $"blogs/{blog.Id}",
            BlogTestData.NieuwBlog("test voor gast"));

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task update_ongeldig_400()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");
        var blog = await MaakBlog("testblog");

        // Act
        var response = await client.PutAsJsonAsync(
            $"blogs/{blog.Id}",
            BlogTestData.NieuwBlog(null));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Put_met_goede_rol_204(string rol)
    {
        // Arrange
        var blog = await MaakBlog("Oude titel");
        var client = await factory.MaakClientAsync(rol);

        // Act
        var response = await client.PutAsJsonAsync(
            $"blogs/{blog.Id}",
            BlogTestData.NieuwBlog("test titel voor update"));

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var target = await client.GetFromJsonAsync<CreateBlogResponse>($"blogs/{blog.Id}");
        Assert.Equal("test titel voor update", target!.Title);
    }

    [Fact]
    public async Task Put_OnbekendId_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");

        // Act
        var response = await client.PutAsJsonAsync(
            "/blogs/999999",
            BlogTestData.NieuwBlog("Bestaat niet"));

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Zonder_Login_401()
    {
        // Arrange
        var blog = await MaakBlog("delete test blog");
        var client = await factory.MaakClientAsync();

        // Act
        var response = await client.DeleteAsync($"/blogs/{blog.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_Gast_403()
    {
        // Arrange
        var blog = await MaakBlog("delete test blog");
        var client = await factory.MaakClientAsync("Gast");

        // Act
        var response = await client.DeleteAsync($"/blogs/{blog.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Delete_met_goede_rol_204(string rol)
    {
        // Arrange
        var blog = await MaakBlog("test verwijder blog");
        var client = await factory.MaakClientAsync(rol);

        // Act
        var response = await client.DeleteAsync($"blogs/{blog.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var target = await client.GetAsync($"blogs/{blog.Id}");
        Assert.Equal(HttpStatusCode.NotFound, target.StatusCode);
    }

    [Fact]
    public async Task Delete_OnbekendId_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");

        // Act
        var response = await client.DeleteAsync("/blogs/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Patch_zonder_login_401()
    {
        // Arrange
        var client = await factory.MaakClientAsync();
        var blog = await MaakBlog("testblog");
        var title = "patch title";

        // Act
        var response = await client.PatchAsJsonAsync($"blogs/{blog.Id}", new { title });

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Patch_als_gast_401()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Gast");
        var blog = await MaakBlog("testblog");
        var title = "patch title";

        // Act
        var response = await client.PatchAsJsonAsync($"blogs/{blog.Id}", new { title });

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Theory]
    [InlineData("Editor")]
    [InlineData("Admin")]
    public async Task Patch_met_goede_rol_204(string rol)
    {
        // Arrange
        var client = await factory.MaakClientAsync(rol);
        var blog = await MaakBlog("oude titel test blog voor patch");
        var title = "patch title";

        // Act
        var response = await client.PatchAsJsonAsync($"blogs/{blog.Id}", new { title });

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var target = await client.GetFromJsonAsync<CreateBlogResponse>($"blogs/{blog.Id}");
        Assert.Equal(title, target!.Title);
    }

    [Fact]
    public async Task Patch_OnbekendId_404()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");
        var title = "patch title";

        // Act
        var response = await client.PatchAsJsonAsync("blogs/90909", new { title });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

        [Fact]
    public async Task Patch_ongeldig_403()
    {
        // Arrange
        var client = await factory.MaakClientAsync("Editor");
        var blog = await MaakBlog("patch ongeldig");
        var newDate = "hi";
        
        // Act
        var response = await client.PatchAsJsonAsync($"blogs/{blog.Id}", new {date = newDate} );

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
