using data.Policies;

namespace functions.UserManager.RegisterUser;


public static class RegisterUserEndpoint
{
    public static RouteHandlerBuilder MapRegisterUserEndpoint(this RouteGroupBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.MapPost("/register", RegisterUserHandler.RegisterUserHandle)
        .WithName("RegisterUser");
    }
}