using data.Policies;

namespace functions.UserManager.LoginUser;


public static class LoginUserEndpoint
{
    public static RouteHandlerBuilder MapLoginUserEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.MapPost("/login", LoginUserHandler.LoginUserHandle)
        .WithName("LoginUser");
    }
}