using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
namespace AppStore.Controllers;

public class HomeController(ILibroService libroService) : Controller
{
    private readonly ILibroService _libroService = libroService;

    public IActionResult Index(string term = "", int currentPage = 1)
    {
         var libroListVm = _libroService.List(term, true, currentPage);

         return View(libroListVm);
    }

    public IActionResult LibroDetail(int libroId)
    {
        var libro = _libroService.GetById(libroId);
        return View(libro);
    }


    // Redireccionar hacia la pagina about
    public IActionResult About()
    {
        return View();
    }

    
}