namespace AppStore.Models.DTO;
using AppStore.Models.Domain;


public class LibroListvm
{
    public IQueryable<Libro>? LibroList { get; set;}

    public int PageSize { get; set; }

    public int currentPage { get; set; }

    public int TotalPage { get; set; }

    public string? Term { get; set; }
}