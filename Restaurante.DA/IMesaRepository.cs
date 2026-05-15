using Restaurante.Model;

namespace Restaurante.DA
{
    /// <summary>
    /// Contrato de acceso a datos para la entidad Mesa.
    /// Mantiene la capa BL desacoplada de EF Core.
    /// </summary>
    public interface IMesaRepository
    {
        IReadOnlyList<Mesa> ObtenerTodas();
        IReadOnlyList<Mesa> ObtenerPorEstado(EstadoMesa estado);
        Mesa? ObtenerPorId(int id);
        void Agregar(Mesa mesa);
        void Actualizar(Mesa mesa);
    }
}
