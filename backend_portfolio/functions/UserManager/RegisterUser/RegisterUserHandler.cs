using Models.user;
using Microsoft.AspNetCore.Identity;
using data.DbSeeder;

namespace functions.UserManager.RegisterUser;

public static class RegisterUserHandler
{

    public static async Task<IResult> RegisterUserHandle(
        RegisterUserRequest request, UserManager<User> userManager
    )
    {
        var user = new User { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
            return TypedResults.ValidationProblem(result.Errors.
            GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray())
        );

        await userManager.AddToRoleAsync(user, "Gast");

    
        return TypedResults.Ok(new RegisterUserResponse(user.Id, user.Email)
        );

        // var response = new RegisterUserResponse(user.Id, user.Email);
        // return TypedResults.CreatedAtRoute(
        //     response,
        //     routeName: "GetUser",
        //     routeValues: new { id = user.Id });

    }
}