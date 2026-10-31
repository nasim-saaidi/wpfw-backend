using System.ComponentModel.DataAnnotations;

namespace functions.PatchBlog;

public sealed class PatchBlogRequest
{
    public string? Title{get; init;} 


    
        public string? Content{get; set;}

    [Range(typeof(DateTime), "1888T00:00:00", "2100T00:00:00")]
    public DateTime? Date{get; set;}

}