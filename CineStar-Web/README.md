# CineStar – ASP.NET Core MVC + SQL Server

Páginas: Inicio, Nuestros Cines (`/Cines`), Cine (`/Cines/Detalle/{id}`),
Cartelera / Próximos estrenos (`/Peliculas/Index/cartelera|estrenos`) y Película (`/Peliculas/Detalle/{id}`).
Diagnóstico de conexión a la BD: `/Home/Estado`.

## 1. Base de datos
Ejecutar `CineStar_SQL_Hosting.sql` sobre la BD `db_ace3c6_cinestar`
(con SSMS u otro cliente SQL).

## 2. Ejecutar en local
1. Requisito: .NET SDK 8 (si tienes otro, cambia `TargetFramework` en `CineStar.csproj`).
2. Configurar la cadena `cnCineStar` en `appsettings.json` (o en `appsettings.Local.json`, ignorado por git).
3. `dotnet run` y abrir la URL que indica la consola.

## 3. Publicar en Azure App Service
- Crear Web App: Runtime **.NET 8 (LTS)**, cualquier región.
- Portal Azure > Web App > **Environment variables / Connection strings**:
  nombre `cnCineStar`, tipo `Custom` (o `SQLAzure`), valor = cadena de conexión de la BD.
- Publicar desde Visual Studio (clic derecho > Publish > Azure) o con **Deployment Center** conectado a GitHub.
- Probar `https://TU-APP.azurewebsites.net/Home/Estado`.

## 4. Entrega
Editar `link-azure.txt` con la URL real de Azure y el link del repositorio, hacer commit y push.
