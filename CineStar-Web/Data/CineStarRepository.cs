using System.Data;
using System.Globalization;
using CineStar.Models;
using Microsoft.Data.SqlClient;

namespace CineStar.Data;

/// <summary>Acceso a datos con ADO.NET usando los procedimientos almacenados de la BD CineStar.</summary>
public class CineStarRepository
{
    private readonly string _cadena;

    public CineStarRepository(IConfiguration config)
    {
        _cadena = config.GetConnectionString("cnCineStar")
            ?? throw new InvalidOperationException("Falta la cadena de conexión 'cnCineStar'.");
    }

    // Las columnas son CHAR(n): vienen rellenas de espacios, por eso se hace Trim.
    private static string Txt(SqlDataReader r, string col)
    {
        var v = r[col];
        return v == DBNull.Value ? "" : Convert.ToString(v)!.Trim();
    }

    // "S/. 5.0000" -> "S/. 5.00"
    private static string FormatearPrecio(string s)
    {
        var numero = s.Replace("S/.", "").Trim();
        return decimal.TryParse(numero, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)
            ? "S/. " + d.ToString("0.00", CultureInfo.InvariantCulture)
            : s;
    }

    private List<T> Ejecutar<T>(string sp, Func<SqlDataReader, T> mapa, params SqlParameter[] parametros)
    {
        var lista = new List<T>();
        using var cn = new SqlConnection(_cadena);
        using var cmd = new SqlCommand(sp, cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        using var dr = cmd.ExecuteReader();
        while (dr.Read()) lista.Add(mapa(dr));
        return lista;
    }

    private static Cine MapCine(SqlDataReader r) => new()
    {
        Id = Convert.ToInt32(r["id"]),
        RazonSocial = Txt(r, "RazonSocial"),
        Salas = Convert.ToInt32(r["Salas"]),
        Direccion = Txt(r, "Direccion"),
        Telefonos = Txt(r, "Telefonos"),
        Distrito = Txt(r, "Detalle")
    };

    public List<Cine> GetCines() => Ejecutar("sp_getCines", MapCine);

    public Cine? GetCine(int id) =>
        Ejecutar("sp_getCine", MapCine, new SqlParameter("@id", id)).FirstOrDefault();

    public List<Tarifa> GetCineTarifas(int idCine) =>
        Ejecutar("sp_getCineTarifas",
            r => new Tarifa { DiasSemana = Txt(r, "DiasSemana"), Precio = FormatearPrecio(Txt(r, "Precio")) },
            new SqlParameter("@idCine", idCine));

    public List<CinePelicula> GetCinePeliculas(int idCine) =>
        Ejecutar("sp_getCinePeliculas",
            r => new CinePelicula { Titulo = Txt(r, "Titulo"), Horarios = Txt(r, "Horarios") },
            new SqlParameter("@idCine", idCine));

    // idEstado: 1 = Cartelera, 2 = Próximo estreno
    public List<Pelicula> GetPeliculas(int idEstado) =>
        Ejecutar("sp_getPeliculas",
            r => new Pelicula
            {
                Id = Convert.ToInt32(r["id"]),
                Titulo = Txt(r, "Titulo"),
                Link = Txt(r, "Link"),
                Sinopsis = Txt(r, "Sinopsis"),
                IdEstado = idEstado
            },
            new SqlParameter("@idEstado", idEstado));

    public Pelicula? GetPelicula(int id) =>
        Ejecutar("sp_getPelicula",
            r => new Pelicula
            {
                Id = Convert.ToInt32(r["id"]),
                Titulo = Txt(r, "Titulo"),
                FechaEstreno = Txt(r, "FechaEstreno"),
                Director = Txt(r, "Director"),
                IdEstado = Convert.ToInt32(r["idEstado"]),
                Link = Txt(r, "Link"),
                Reparto = Txt(r, "Reparto"),
                Sinopsis = Txt(r, "Sinopsis"),
                Generos = Txt(r, "Geneross") // así se llama la columna en sp_getPelicula
            },
            new SqlParameter("@id", id)).FirstOrDefault();
}
