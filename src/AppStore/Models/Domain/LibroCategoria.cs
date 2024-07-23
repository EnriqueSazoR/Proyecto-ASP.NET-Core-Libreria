namespace AppStore.Models.Domain;


public class LibroCategoria
{

    public int Id { get; set; }
    public int CategoriaId { get; set; }
    public Categoria? categoria {get; set;}
    public int LibroId { get; set; }
    public Libro? libro {get; set;}
}