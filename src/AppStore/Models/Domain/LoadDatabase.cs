using Microsoft.AspNetCore.Identity;
namespace AppStore.Models.Domain;


public class LoadDatabase
{
    // crear metodo estatico para ingresar data (usuario, categoria)
    public static async Task InsertarData(DataBaseContext context, 
                                          UserManager<ApplicationUser> usuarioManager, 
                                          RoleManager<IdentityRole> roleManager)
    {
        if(!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new IdentityRole("ADMIN"));
        }

        if(!usuarioManager.Users.Any())
        {
            var usuario = new ApplicationUser{
                Nombre = "Luis Sazo",
                Email = "Lsazo@gmail.com",
                UserName = "Lsazor"
            };

           await usuarioManager.CreateAsync(usuario, "PasswordLuis123$");
           await usuarioManager.AddToRoleAsync(usuario, "ADMIN");
        }

        if(!context.Categorias!.Any())
        {
           await context.Categorias.AddRangeAsync(
                new Categoria{Nombre = "Drama"},
                new Categoria{Nombre = "Comedia"},
                new Categoria{Nombre = "Accion"},
                new Categoria{Nombre = "Terror"},
                new Categoria{Nombre = "Aventura"}
            );

             await context.SaveChangesAsync();
        }

        if(!context.Libros!.Any())
        {
            await context.Libros.AddRangeAsync(
               new Libro{
                Titulo = "El Quijote de la Mancha",
                CreateDate = "06/06/2020",
                Imagen = "quijote.jpg",
                Autor = "Miguel de Cervantes Saavedra"},

                new Libro{
                Titulo = "Harry Potter",
                CreateDate = "06/01/2021",
                Imagen = "harry.jpg",
                Autor = "Juan de la Vega"}

            );

            await context.SaveChangesAsync();
        }

        if(!context.LibroCategorias!.Any())
        {
            await context.LibroCategorias.AddRangeAsync(
                new LibroCategoria{CategoriaId = 1, LibroId = 1},
                new LibroCategoria{CategoriaId = 2, LibroId = 2}
            );

            await context.SaveChangesAsync();
        }
    }
}