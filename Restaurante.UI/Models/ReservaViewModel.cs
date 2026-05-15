using System.ComponentModel.DataAnnotations;

namespace Restaurante.UI.Models
{
    /// <summary>
    /// Modelo de presentación del formulario "Reservar mesa".
    /// Sus atributos sirven exclusivamente para la validación cliente/servidor
    /// mediante asp-validation-for; toda la lógica del cálculo y la actualización
    /// se delega al servicio en la capa BL.
    /// </summary>
    public class ReservaViewModel
    {
        public int MesaId { get; set; }

        [Display(Name = "Número de mesa")]
        public int NumeroMesa { get; set; }

        [Display(Name = "Precio por persona")]
        [DataType(DataType.Currency)]
        public decimal PrecioPorPersona { get; set; }

        [Required(ErrorMessage = "La identificación del cliente es obligatoria.")]
        [Display(Name = "Identificación del cliente")]
        public string IdentificacionCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [Display(Name = "Nombre del cliente")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de la reserva es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de la reserva")]
        public DateTime FechaReserva { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La cantidad de comensales es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de comensales debe ser mayor a 0.")]
        [Display(Name = "Cantidad de comensales")]
        public int CantidadComensales { get; set; }
    }
}
