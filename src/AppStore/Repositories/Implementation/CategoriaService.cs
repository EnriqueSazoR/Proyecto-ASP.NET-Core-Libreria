using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
namespace AppStore.Repositories.Implementation;

public class CategoriaService(DataBaseContext ctx) : ICategoriaService
{
    private readonly DataBaseContext ctx = ctx;

    public IQueryable<Categoria> List()
    {
        return ctx.Categorias.AsQueryable();
    }
}