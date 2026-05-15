namespace Restaurante.BL.Modelos
{
    /// <summary>
    /// DTO con los datos requeridos para reservar una mesa.
    /// Se utiliza para que la UI envíe únicamente la información mínima
    /// y la capa BL aplique las reglas de negocio (cálculo del depósito,
    /// cambio de estado, persistencia).
    /// </summary>
    public class DatosReserva
    {
        public int MesaId { get; set; }
        public string IdentificacionCliente { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public DateTime FechaReserva { get; set; }
        public int CantidadComensales { get; set; }
    }
}
