using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using data.Policies;

namespace functions.CreateBlog;

public static class CreateBlogEndpoint
{
    public static RouteHandlerBuilder MapCreateBlogEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.MapPost("", CreateBlogHandler.CreateBlogHandle)
        .WithName("CreateBlog").RequireAuthorization(Policies.CanCreateItem);
    }
}