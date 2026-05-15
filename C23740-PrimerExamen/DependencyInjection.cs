using Microsoft.Extensions.DependencyInjection;

namespace Restaurante.BL
{
    /// <summary>
    /// Registro centralizado de la capa de lógica de negocio.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection servicios)
        {
            servicios.AddScoped<IMesaService, MesaService>();
            return servicios;
        }
    }
}
