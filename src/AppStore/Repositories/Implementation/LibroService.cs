using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;
namespace AppStore.Repositories.Implementation;

// clase para implementar interfaz ILibro
public class LibroService(DataBaseContext ctxParameter) : ILibroService
{

    private readonly DataBaseContext ctx = ctxParameter;

    // Metodo para agregar un nuevo libro
    public bool Add(Libro libro)
    {
        try{

            ctx.Libros.Add(libro);
            ctx.SaveChanges();
            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria
                {
                    LibroId = libro.Id,
                    CategoriaId = categoriaId
                };
                ctx.LibroCategorias.Add(libroCategoria);
            }
        }catch(Exception)
        {
            return false;
        }
        ctx.SaveChanges();

        return true;
        
    }

    // Metodo para eliminar un libro mediante el id
    public bool Delete(int id)
    {
        try
        {
            var data = GetById(id);
            if(data is null)
            {
                return false;
            }

            var libroCategorias = ctx.LibroCategorias.Where(a => a.LibroId == data.Id);
            ctx.LibroCategorias.RemoveRange(libroCategorias);
            ctx.Libros.Remove(data);
            ctx.SaveChanges();
            return true;

        }
        catch(Exception)
        {
            return false;
        }

    }

    // Metodo para obtener un libro en concreto mediante el id
    public Libro GetById(int id)
    {
        return ctx.Libros.Find(id)!;
    }

    // metodo para crear paginación
    public LibroListvm List(string term = "", bool paging = false, int currentPage = 0)
    {
        var data = new LibroListvm();
        var list = ctx.Libros.ToList();
        if(!string.IsNullOrEmpty(term))
        {
            term = term.ToLower();
            list = list.Where(a => a.Titulo!.StartsWith(term, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }

        if(paging)
        {
            int pageSize = 5;
            int count = list.Count;
            int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            list = list.Skip((currentPage-1) * pageSize).Take(pageSize).ToList();
            data.PageSize = pageSize;
            data.currentPage = currentPage;
            data.TotalPage = TotalPages;
        }

        foreach(var libro in list)
        {
            var categorias = (
                from categoria in ctx.Categorias
                join lc in ctx.LibroCategorias
                on categoria.Id equals lc.CategoriaId
                where lc.LibroId == libro.Id
                select categoria.Nombre
            ).ToList();

            string categoriaNombres = string.Join(",", categorias);
            libro.CategoriasNames = categoriaNombres;
        }

        data.LibroList = list.AsQueryable();


        return data;
    }

    // Metodo para actulizar un libro
    public bool Update(Libro libro)
    {
        try{
            var categoriasParaEliminar = ctx.LibroCategorias.Where(a => a.LibroId == libro.Id);
            foreach(var categoria in categoriasParaEliminar)
            {
                ctx.LibroCategorias.Remove(categoria);
            }
            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria {CategoriaId = categoriaId, LibroId = libro.Id};
                ctx.LibroCategorias.Add(libroCategoria);
            }
            ctx.Libros.Update(libro);
            ctx.SaveChanges();
            return true;

        }
        catch(Exception)
        {
            return false;
        }
    }

    // Metodo para obtener una lista de las categorias que puede tener un libro
    public List<int> GetCategoriaByLibroId(int libroId)
    {
        return ctx.LibroCategorias.Where(a => a.LibroId == libroId).Select(a => a.CategoriaId).ToList();
    }
}