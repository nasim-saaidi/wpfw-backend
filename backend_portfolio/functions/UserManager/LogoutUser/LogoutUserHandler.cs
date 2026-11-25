using Models.user;
using Microsoft.AspNetCore.Identity;
using data.DbSeeder;
using Microsoft.AspNetCore.Mvc;

namespace functions.UserManager.LogoutUser;

public static class LogoutUserHandler
{

    public static async Task<IResult> LogoutUserHandle(
          SignInManager<User> signInManager
    )
    {
      
        
            await signInManager.SignOutAsync();
            return Results.Ok();
        
    }
}