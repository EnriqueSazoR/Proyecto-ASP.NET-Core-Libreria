using AppStore.Models.Domain;
namespace AppStore.Repositories.Abstract;

public interface ICategoriaService
{
    IQueryable<Categoria> List();
}