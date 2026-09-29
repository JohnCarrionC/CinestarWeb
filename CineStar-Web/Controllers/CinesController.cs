using CineStar.Data;
using CineStar.Models;
using Microsoft.AspNetCore.Mvc;

namespace CineStar.Controllers;

public class CinesController : Controller
{
    private readonly CineStarRepository _repo;
    private readonly IWebHostEnvironment _env;

    public CinesController(CineStarRepository repo, IWebHostEnvironment env)
    {
        _repo = repo;
        _env = env;
    }

    // /Cines  -> lista de cines
    public IActionResult Index() => View(_repo.GetCines());

    // /Cines/Detalle/1  -> un cine con tarifas y películas
    public IActionResult Detalle(int id)
    {
        var cine = _repo.GetCine(id);
        if (cine == null) return NotFound();

        var vm = new CineDetalleViewModel
        {
            Cine = cine,
            Tarifas = _repo.GetCineTarifas(id),
            Peliculas = _repo.GetCinePeliculas(id),
            TieneImagen2 = ImagenExiste(id, 2),
            TieneImagen3 = ImagenExiste(id, 3)
        };
        return View(vm);
    }

    private bool ImagenExiste(int idCine, int n) =>
        System.IO.File.Exists(Path.Combine(_env.WebRootPath, "img", "cine", $"{idCine}.{n}.jpg"));
}
