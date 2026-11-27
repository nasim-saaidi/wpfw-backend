using data.Policies;

namespace functions.UserManager.Me;


public static class GetUserEndpoint
{
    public static RouteHandlerBuilder MapGetUserEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.MapGet("/Me", GetUserHandler.GetUserHandle)
        .WithName("GetUser").RequireAuthorization();
    }
}