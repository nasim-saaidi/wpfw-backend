using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models.user;
using TestAPI.Tests;
using functions.CreateProject;
using functions.GetProject;

namespace backend_portfolio.Tests;


public static class TestData
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
public class ProjectsTests(ApiFactory factory)
{

    private async Task<GetProjectResponse> MaakProject(string titel)
    {
        var user = await factory.MaakClientAsync("Editor");
        var response = await user.PostAsJsonAsync("projects", TestData.NieuwProject);
        return (await response.Content.ReadFromJsonAsync<GetProjectResponse>())!;
    }
    
    
}