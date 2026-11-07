using System.ComponentModel.DataAnnotations;

namespace functions.CreateBlog;

public sealed class CreateBlogRequest
{

      [Required]
    public string Title{get; init;} = string.Empty;

    
    [Required]
        public string Content{get; set;} = string.Empty;

    [Required]
        [Range(typeof(DateTime), "1888-01-01T00:00:00", "2100-12-31T00:00:00")]
    public DateTime Date{get; set;}

    
    public string? InternalNotes{get; set;}
}