using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using data.Policies;


namespace functions.CreateProject;

public static class CreateProjectEndpoint
{
    public static RouteHandlerBuilder MapCreateProjectEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.MapPost("", CreateProjectHandler.CreateProjectHandle)
        .WithName("CreateProject").RequireAuthorization();
    }
}