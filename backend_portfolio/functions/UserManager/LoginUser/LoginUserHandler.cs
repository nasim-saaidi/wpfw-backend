using Models.user;
using Microsoft.AspNetCore.Identity;
using data.DbSeeder;

namespace functions.UserManager.LoginUser;

public static class LoginUserHandler
{

    public static async Task<IResult> LoginUserHandle(
        LoginUserRequest request, SignInManager<User> signInManager
    )
    {
       var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent: false, lockoutOnFailure: false);
       return result.Succeeded ? TypedResults.Ok() : TypedResults.Unauthorized();
    }
}