using System.ComponentModel.DataAnnotations;

namespace functions.UpdateBlog;

public sealed class UpdateBlogRequest
{

      [Required]
    public string Title{get; init;} = string.Empty;

    
    [Required]
    
        public string Content{get; set;} = string.Empty;

    [Required]
    [Range(typeof(DateTime), "1888T00:00:00", "2100T00:00:00")]
    public DateTime Date{get; set;}

}