using System.ComponentModel.DataAnnotations;

namespace functions.CreateBlog;

public sealed class CreateBlogRequest
{

      [Required]
    public string Title{get; init;} = string.Empty;

    
    [Required]
    [Range(typeof(DateTime), "1888T00:00:00", "2100T00:00:00")]
        public string Content{get; set;} = string.Empty;

    [Required]
    
    public DateTime Date{get; set;}

}