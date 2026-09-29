using CineStar.Data;
using Microsoft.AspNetCore.Mvc;

namespace CineStar.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Error() => View();

    // Diagnóstico rápido: /Home/Estado  -> indica si la web llega a la BD.
    public IActionResult Estado([FromServices] CineStarRepository repo)
    {
        try
        {
            var n = repo.GetCines().Count;
            return Content($"OK - conexión a la BD correcta. Cines encontrados: {n}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "ERROR de conexión a la BD: " + ex.Message);
        }
    }
}
