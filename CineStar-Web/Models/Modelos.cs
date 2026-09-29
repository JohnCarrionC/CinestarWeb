namespace CineStar.Models;

public class Cine
{
    public int Id { get; set; }
    public string RazonSocial { get; set; } = "";
    public int Salas { get; set; }
    public string Direccion { get; set; } = "";
    public string Telefonos { get; set; } = "";
    public string Distrito { get; set; } = "";

    // "Cinestar Excelsior" -> "Excelsior"
    public string NombreCorto => RazonSocial.Replace("Cinestar ", "");
}

public class Tarifa
{
    public string DiasSemana { get; set; } = "";
    public string Precio { get; set; } = "";
}

public class CinePelicula
{
    public string Titulo { get; set; } = "";
    public string Horarios { get; set; } = "";
}

public class Pelicula
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string FechaEstreno { get; set; } = "";
    public string Director { get; set; } = "";
    public int IdEstado { get; set; }
    public string Link { get; set; } = "";
    public string Reparto { get; set; } = "";
    public string Sinopsis { get; set; } = "";
    public string Generos { get; set; } = "";

    private static readonly string[] Meses =
    {
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    };

    // "11/01/2018" -> "11 de Enero del 2018"
    public string FechaEstrenoTexto
    {
        get
        {
            var p = FechaEstreno.Split('/');
            if (p.Length == 3 && int.TryParse(p[0], out var d) && int.TryParse(p[1], out var m)
                && m >= 1 && m <= 12)
            {
                return $"{d} de {Meses[m - 1]} del {p[2]}";
            }
            return FechaEstreno;
        }
    }

    // "Aventura,Acción" -> "Aventura / Acción"
    public string GenerosTexto => Generos.Replace(",", " / ");

    public string SinopsisCorta =>
        Sinopsis.Length <= 220 ? Sinopsis : Sinopsis.Substring(0, 220).TrimEnd() + " ...";
}

public class CineDetalleViewModel
{
    public Cine Cine { get; set; } = new();
    public List<Tarifa> Tarifas { get; set; } = new();
    public List<CinePelicula> Peliculas { get; set; } = new();
    public bool TieneImagen2 { get; set; }
    public bool TieneImagen3 { get; set; }
}
