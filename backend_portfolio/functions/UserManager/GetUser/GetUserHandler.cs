using Models.user;
using Microsoft.AspNetCore.Identity;
using data.DbSeeder;
using System.Security.Claims;

namespace functions.UserManager.Me;

public static class GetUserHandler
{

    public static async Task<IResult> GetUserHandle(
        ClaimsPrincipal user
    )
    {
       return TypedResults.Ok(new { name = user.Identity?.Name });

    }
}