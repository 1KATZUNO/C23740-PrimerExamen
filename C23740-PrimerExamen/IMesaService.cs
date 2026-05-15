using Restaurante.BL.Modelos;
using Restaurante.Model;

namespace Restaurante.BL
{
    /// <summary>
    /// Contrato del servicio de gestión de mesas. Concentra toda la lógica
    /// de negocio (filtros, validaciones y cálculos) para mantener la UI
    /// libre de reglas y la DA enfocada únicamente en persistencia.
    /// </summary>
    public interface IMesaService
    {
        // --- Módulo de Registro de mesas ---
        IReadOnlyList<Mesa> ObtenerTodas(string? filtroNumeroMesa = null);
        Mesa? ObtenerPorId(int id);
        void RegistrarMesa(Mesa mesa);
        void EditarMesa(Mesa mesa);

        // --- Módulo de Mesas Disponibles ---
        IReadOnlyList<Mesa> ObtenerDisponibles(string? filtroNumeroMesa = null);
        decimal CalcularDepositoGarantia(decimal precioPorPersona, int cantidadComensales);
        void ReservarMesa(DatosReserva datos);

        // --- Módulo de Mesas Reservadas ---
        IReadOnlyList<Mesa> ObtenerReservadas(string? filtroNombreCliente = null);
        void LiberarMesa(int id);
    }
}
