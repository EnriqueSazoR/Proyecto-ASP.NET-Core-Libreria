namespace AppStore.Models.Domain;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string? Nombre { get; set; }
}