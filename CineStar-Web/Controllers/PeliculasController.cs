using CineStar.Data;
using Microsoft.AspNetCore.Mvc;

namespace CineStar.Controllers;

public class PeliculasController : Controller
{
    private readonly CineStarRepository _repo;

    public PeliculasController(CineStarRepository repo) => _repo = repo;

    // /Peliculas/Index/cartelera   o   /Peliculas/Index/estrenos
    public IActionResult Index(string? id)
    {
        bool estrenos = string.Equals(id, "estrenos", StringComparison.OrdinalIgnoreCase);
        ViewBag.Titulo = estrenos ? "Próximos Estrenos" : "Cartelera";
        return View(_repo.GetPeliculas(estrenos ? 2 : 1));
    }

    // /Peliculas/Detalle/1
    public IActionResult Detalle(int id)
    {
        var p = _repo.GetPelicula(id);
        if (p == null) return NotFound();
        ViewBag.Titulo = p.IdEstado == 2 ? "Próximos Estrenos" : "Cartelera";
        return View(p);
    }
}
