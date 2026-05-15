using Restaurante.BL.Modelos;
using Restaurante.DA;
using Restaurante.Model;

namespace Restaurante.BL
{
    /// <summary>
    /// Implementación de las reglas de negocio para la gestión de mesas.
    /// La UI debe consumir únicamente esta capa; cualquier cálculo o filtro
    /// vive aquí (no en la UI ni en la DA).
    /// </summary>
    public class MesaService : IMesaService
    {
        // Umbrales y porcentajes definidos por el enunciado para el cálculo
        // del depósito de garantía. Se mantienen como constantes nombradas
        // para evitar "magic numbers" en la fórmula.
        private const int UmbralComensalesIntermedio = 4;
        private const int UmbralComensalesAlto = 8;
        private const decimal PorcentajeDepositoGrupoPequeno = 1.00m;   // 100 %
        private const decimal PorcentajeDepositoGrupoMediano = 0.75m;   //  75 %
        private const decimal PorcentajeDepositoGrupoGrande = 0.50m;    //  50 %

        private readonly IMesaRepository _repositorio;

        public MesaService(IMesaRepository repositorio)
        {
            _repositorio = repositorio;
        }

        // ----------------------- Registro de mesas -----------------------

        public IReadOnlyList<Mesa> ObtenerTodas(string? filtroNumeroMesa = null)
        {
            var mesas = _repositorio.ObtenerTodas();
            return AplicarFiltroNumeroMesa(mesas, filtroNumeroMesa);
        }

        public Mesa? ObtenerPorId(int id) => _repositorio.ObtenerPorId(id);

        public void RegistrarMesa(Mesa mesa)
        {
            ArgumentNullException.ThrowIfNull(mesa);

            // Toda mesa nueva se registra con estado Disponible (1).
            mesa.Estado = (int)EstadoMesa.Disponible;
            mesa.IdentificacionCliente = null;
            mesa.NombreCliente = null;
            mesa.FechaReserva = null;
            mesa.CantidadComensales = null;
            mesa.DepositoDeGarantia = null;

            _repositorio.Agregar(mesa);
        }

        public void EditarMesa(Mesa mesa)
        {
            ArgumentNullException.ThrowIfNull(mesa);

            var actual = _repositorio.ObtenerPorId(mesa.Id)
                ?? throw new KeyNotFoundException($"No existe una mesa con Id {mesa.Id}.");

            // Solo se permite editar los cuatro campos requeridos según el
            // enunciado. El estado y los datos de reserva no se tocan aquí.
            actual.NumeroMesa = mesa.NumeroMesa;
            actual.Capacidad = mesa.Capacidad;
            actual.Seccion = mesa.Seccion;
            actual.PrecioPorPersona = mesa.PrecioPorPersona;

            _repositorio.Actualizar(actual);
        }

        // ----------------------- Mesas Disponibles -----------------------

        public IReadOnlyList<Mesa> ObtenerDisponibles(string? filtroNumeroMesa = null)
        {
            var disponibles = _repositorio.ObtenerPorEstado(EstadoMesa.Disponible);
            return AplicarFiltroNumeroMesa(disponibles, filtroNumeroMesa);
        }

        /// <summary>
        /// Calcula el depósito de garantía según las reglas del enunciado:
        ///   < 4 comensales   → 100 % * precio * comensales.
        ///   4 .. 7 comensales →  75 % * precio * comensales.
        ///   ≥ 8 comensales   →  50 % * precio * comensales.
        /// </summary>
        public decimal CalcularDepositoGarantia(decimal precioPorPersona, int cantidadComensales)
        {
            if (precioPorPersona <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(precioPorPersona),
                    "El precio por persona debe ser mayor a 0.");
            }
            if (cantidadComensales <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cantidadComensales),
                    "La cantidad de comensales debe ser mayor a 0.");
            }

            decimal porcentaje = cantidadComensales switch
            {
                < UmbralComensalesIntermedio => PorcentajeDepositoGrupoPequeno,
                < UmbralComensalesAlto => PorcentajeDepositoGrupoMediano,
                _ => PorcentajeDepositoGrupoGrande
            };

            return porcentaje * precioPorPersona * cantidadComensales;
        }

        public void ReservarMesa(DatosReserva datos)
        {
            ArgumentNullException.ThrowIfNull(datos);

            var mesa = _repositorio.ObtenerPorId(datos.MesaId)
                ?? throw new KeyNotFoundException($"No existe una mesa con Id {datos.MesaId}.");

            if (mesa.Estado != (int)EstadoMesa.Disponible)
            {
                throw new InvalidOperationException(
                    "Solo se pueden reservar mesas que se encuentren en estado Disponible.");
            }

            mesa.IdentificacionCliente = datos.IdentificacionCliente;
            mesa.NombreCliente = datos.NombreCliente;
            mesa.FechaReserva = datos.FechaReserva;
            mesa.CantidadComensales = datos.CantidadComensales;
            mesa.DepositoDeGarantia = CalcularDepositoGarantia(mesa.PrecioPorPersona, datos.CantidadComensales);
            mesa.Estado = (int)EstadoMesa.Reservada;

            _repositorio.Actualizar(mesa);
        }

        // ----------------------- Mesas Reservadas ------------------------

        public IReadOnlyList<Mesa> ObtenerReservadas(string? filtroNombreCliente = null)
        {
            var reservadas = _repositorio.ObtenerPorEstado(EstadoMesa.Reservada);

            if (string.IsNullOrWhiteSpace(filtroNombreCliente))
            {
                return reservadas;
            }

            var termino = filtroNombreCliente.Trim().ToLowerInvariant();
            return reservadas
                .Where(m => (m.NombreCliente ?? string.Empty).ToLowerInvariant().Contains(termino))
                .ToList();
        }

        public void LiberarMesa(int id)
        {
            var mesa = _repositorio.ObtenerPorId(id)
                ?? throw new KeyNotFoundException($"No existe una mesa con Id {id}.");

            // Liberar la mesa: vuelve a estado Disponible y se limpian los
            // datos de la reserva anterior para que la lista quede consistente.
            mesa.Estado = (int)EstadoMesa.Disponible;
            mesa.IdentificacionCliente = null;
            mesa.NombreCliente = null;
            mesa.FechaReserva = null;
            mesa.CantidadComensales = null;
            mesa.DepositoDeGarantia = null;

            _repositorio.Actualizar(mesa);
        }

        // ----------------------- Helpers privados ------------------------

        private static IReadOnlyList<Mesa> AplicarFiltroNumeroMesa(
            IEnumerable<Mesa> mesas,
            string? filtroNumeroMesa)
        {
            if (string.IsNullOrWhiteSpace(filtroNumeroMesa))
            {
                return mesas.ToList();
            }

            var termino = filtroNumeroMesa.Trim();
            return mesas
                .Where(m => m.NumeroMesa.ToString().Contains(termino))
                .ToList();
        }
    }
}
