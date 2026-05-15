using System.ComponentModel.DataAnnotations;

namespace Restaurante.Model
{
    /// <summary>
    /// Entidad que representa una mesa del restaurante. Las columnas reflejan
    /// el diseño existente de la tabla dbo.Mesas en la base de datos
    /// ExamenRestaurante. El diseño de la BD no puede modificarse.
    /// </summary>
    public class Mesa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de mesa es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de mesa debe ser mayor a 0.")]
        [Display(Name = "Número de mesa")]
        public int NumeroMesa { get; set; }

        /// <summary>
        /// Estado actual de la mesa. Mapeado como entero contra la BD; usar
        /// el enum EstadoMesa en código para evitar magic numbers.
        /// </summary>
        [Display(Name = "Estado")]
        public int Estado { get; set; } = (int)EstadoMesa.Disponible;

        [Required(ErrorMessage = "La capacidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0.")]
        [Display(Name = "Capacidad")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "La sección es obligatoria.")]
        [Display(Name = "Sección")]
        public string Seccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio por persona es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio por persona debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Precio por persona")]
        public decimal PrecioPorPersona { get; set; }

        [Display(Name = "Identificación del cliente")]
        public string? IdentificacionCliente { get; set; }

        [Display(Name = "Nombre del cliente")]
        public string? NombreCliente { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de la reserva")]
        public DateTime? FechaReserva { get; set; }

        [Display(Name = "Cantidad de comensales")]
        public int? CantidadComensales { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Depósito de garantía")]
        public decimal? DepositoDeGarantia { get; set; }

        // ----- Propiedades de presentación (no se persisten) -----

        /// <summary>Texto amigable del estado para mostrar en la UI.</summary>
        public string EstadoNombre => Estado switch
        {
            (int)EstadoMesa.Disponible => "Disponible",
            (int)EstadoMesa.Reservada => "Reservada",
            _ => "Desconocido"
        };
    }
}
