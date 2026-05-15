using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurante.DA
{
    /// <summary>
    /// Registro centralizado de la capa de acceso a datos.
    /// La UI no conoce los detalles de EF Core: solo invoca AddDataAccess.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection servicios, string cadenaConexion)
        {
            if (string.IsNullOrWhiteSpace(cadenaConexion))
            {
                throw new ArgumentException(
                    "La cadena de conexión 'DefaultConnection' no puede ser nula o vacía.",
                    nameof(cadenaConexion));
            }

            servicios.AddDbContext<AppDbContext>(opciones =>
                opciones.UseSqlServer(cadenaConexion));

            servicios.AddScoped<IMesaRepository, MesaRepository>();

            return servicios;
        }
    }
}
