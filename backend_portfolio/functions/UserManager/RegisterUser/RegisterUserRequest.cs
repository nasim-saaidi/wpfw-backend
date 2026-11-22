using System.ComponentModel.DataAnnotations;

namespace functions.UserManager.RegisterUser;
public sealed class RegisterUserRequest
{
    [Required, EmailAddress]
    public string Email{get; init;} = string.Empty;

    [Required]
    public string Password {get; init;} = string.Empty;
}

