using System.ComponentModel.DataAnnotations;
namespace AppStore.Models.Domain;

public class Libro
{
    [Key]
    [Required]   // notaciones, agregan funcionalidades a una propiedad
    public int Id { get; set; }

    public string? Titulo { get; set; }
    
    public string? CreateDate { get; set; }

    public string? Imagen { get; set; }

    [Required]
    public string? Autor { get; set; }

    public virtual ICollection<Categoria>? CategoriaRelationList { get; set;}
    public virtual ICollection<LibroCategoria>? LibroCategoriaRelationList { get; set;}
}