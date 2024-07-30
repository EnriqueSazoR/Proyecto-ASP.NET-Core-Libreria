using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
namespace AppStore.Repositories.Implementation;

public class UserAuthenticationService : IUserAuthenticationService
{

    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public UserAuthenticationService(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task<Status> LoginAsync(LoginModel login)
    {
         var status = new Status();
         var user = await userManager.FindByNameAsync(login.Username!);
         
         if(user is null)
         {
            status.StatusCode = 0;
            status.Message = "El username es invalido";
            return status;
         }

         if(!await userManager.CheckPasswordAsync(user, login.Password!))
         {
            status.StatusCode = 0;
            status.Message = "El password es invalido";
            return status;
         }

         var resultado = await signInManager.PasswordSignInAsync(user, login.Password!, true, false);

         if(!resultado.Succeeded)
         {
            status.StatusCode = 0;
            status.Message = "Las credenciales son incorrectas";

         }

         status.StatusCode = 1;
         status.Message = "Login Exitoso";

         return status;


    }

    public  async Task LogoutAsync()
    {
         await signInManager.SignOutAsync();
    }

}