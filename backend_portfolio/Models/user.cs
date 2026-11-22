using Microsoft.AspNetCore.Identity;
namespace Models.user;

public class User : IdentityUser
{
    public int StudentNumber { get; set; }   // from the slide, optional
}