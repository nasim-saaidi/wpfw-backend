using System.ComponentModel.DataAnnotations;

namespace functions.PatchBlog;

public sealed class PatchBlogRequest
{
    public string? Title{get; init;} 


    
        public string? Content{get; set;}

    [Range(typeof(DateTime), "1888-01-01T00:00:00", "2100-12-31T00:00:00")]
    public DateTime? Date{get; set;}

    public string? InternalNotes{get; set;}
}