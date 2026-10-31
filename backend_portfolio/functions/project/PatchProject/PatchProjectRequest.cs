using System.ComponentModel.DataAnnotations;

namespace functions.PatchProject;

public sealed class PatchProjectRequest
{

    public string? Title{get; init;}

    
    
    public string? Description{get; init;}

    
    public string? Category{get; init;}

    [Range(typeof(DateOnly), "1888-01-01", "2100-12-31")]
    public DateOnly? Date{get; init;}


    public string? Repo_link {get; init;} 

  

    public string? Img_alt{get; init;}

    public string? Img_src{get; init;}

}