using System.ComponentModel.DataAnnotations;

namespace functions.UserManager.LoginUser;
public sealed class LoginUserRequest
{
    [Required, EmailAddress]
    public string Email{get; init;} = string.Empty;

    [Required]
    public string Password {get; init;} = string.Empty;
}

