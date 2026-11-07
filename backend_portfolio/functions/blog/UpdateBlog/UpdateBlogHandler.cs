using Microsoft.EntityFrameworkCore;
using main.db;

namespace functions.UpdateBlog;

public static class UpdateBlogHandler
{
    public static async Task<IResult> UpdateBlogHandle(
        int id, UpdateBlogRequest request, PortDB dbContext, 
        CancellationToken cancellationToken
    )
    {
        var blog = await dbContext.Blogs
        .FirstOrDefaultAsync(blog => blog.Id == id, cancellationToken);

        if(blog is null)
        {
            return TypedResults.NotFound();
        }

        
        blog.Title = request.Title;
        blog.Content = request.Content;
        blog.Date = request.Date;
        blog.InternalNotes = request.InternalNotes;
        await dbContext.SaveChangesAsync(cancellationToken);

       return TypedResults.NoContent();
    }
}