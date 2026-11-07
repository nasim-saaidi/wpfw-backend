using Microsoft.AspNetCore.Components.Web;

namespace functions.CreateBlog;
public sealed record class CreateBlogResponse(int Id, string Title, string Content, DateTime Date);