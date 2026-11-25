using System.ComponentModel.DataAnnotations;

namespace functions.UserManager.LogoutUser;
public sealed class LogoutUserRequest
{
    [Required, EmailAddress]
    public string Email{get; init;} = string.Empty;

    [Required]
    public string Password {get; init;} = string.Empty;
}

