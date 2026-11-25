using data.Policies;

namespace functions.UserManager.LogoutUser;


public static class LogoutUserEndpoint
{
    public static RouteHandlerBuilder MapLogoutUserEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.MapPost("/logout", LogoutUserHandler.LogoutUserHandle)
        .WithName("LogoutUser").RequireAuthorization();
    }
}