using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using AppStore.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace AppStore.Controllers;

public class LibroController(ILibroService _libroService, IFileService _fileService, ICategoriaService _categoriaService) : Controller
{
    private readonly ILibroService _libroService = _libroService;
    private readonly IFileService _fileService = _fileService;
    private readonly ICategoriaService _categoriaService = _categoriaService;


    [HttpPost]
    public IActionResult Add(Libro libro)
    {
        libro.CategoriaList = _categoriaService.List()
        .Select(a => new SelectListItem {Text = a.Nombre, Value = a.Id.ToString()});
        if(!ModelState.IsValid)
        {
            return View();
        }

        if(libro.ImageFile != null)
        {
            var resultado = _fileService.SaveImage(libro.ImageFile);
            if(resultado.Item1 == 0)
            {
                TempData["msg"] = "La imagen no pudo guardarse";
                return View(libro);
            }

            var imagenName = resultado.Item2;
            libro.Imagen = imagenName;  
        }
        var resultadoLibro = _libroService.Add(libro);
        if(resultadoLibro)
        {
            TempData["msg"] = "Se agregó el libro correctamente";
            return RedirectToAction(nameof(Add));
        }
        TempData["msg"] = "Errores guardando el libro";
        return View(libro);
    }



    public IActionResult Add()
    {
        var libro = new Libro();
        libro.CategoriaList = _categoriaService.List()
            .Select(a => new SelectListItem {Text = a.Nombre, Value = a.Id.ToString()});

        return View(libro);
    }

    public IActionResult Edit(int id)
    {
        var libro = _libroService.GetById(id);
        var categoriasDeLibro = _libroService.GetCategoriaByLibroId(id);
        var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro);
        libro.MultiCategoriasList = multiSelectListCategorias;

        return View(libro);
    }

    [HttpPost]
    public IActionResult Edit(Libro libro)
    {
        var categoriasDeLibro = _libroService.GetCategoriaByLibroId(libro.Id);
        var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDeLibro);
        libro.MultiCategoriasList = multiSelectListCategorias;

        if(!ModelState.IsValid)
        {
            return View(libro);
        }

        if(libro.ImageFile != null)
        {
            var fileResultado = _fileService.SaveImage(libro.ImageFile);
            if(fileResultado.Item1 == 0)
            {
                TempData["msg"] = "La imagen no fue guardada";
                return View(libro);
            }
            var imagenName = fileResultado.Item2;
            libro.Imagen = imagenName;
        }

        var resultadoLibro = _libroService.Update(libro);
        if(!resultadoLibro)
        {
            TempData["msg"] = "Error, no se pudo actualizar el libro";
            return View(libro);
        }
        TempData["msg"] = "El libro se actualizó exitosamente";
        return View(libro);
         
    }

    public IActionResult LibroList()
    {
        var libros = _libroService.List();
        return View(libros);
    }

    public IActionResult Delete(int id)
    {
        _libroService.Delete(id);
        return RedirectToAction(nameof(LibroList));
    }
}