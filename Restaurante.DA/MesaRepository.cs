using Microsoft.EntityFrameworkCore;
using Restaurante.Model;

namespace Restaurante.DA
{
    public class MesaRepository : IMesaRepository
    {
        private readonly AppDbContext _contexto;

        public MesaRepository(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        public IReadOnlyList<Mesa> ObtenerTodas()
        {
            return _contexto.Mesas
                .AsNoTracking()
                .OrderBy(m => m.NumeroMesa)
                .ToList();
        }

        public IReadOnlyList<Mesa> ObtenerPorEstado(EstadoMesa estado)
        {
            int valor = (int)estado;
            return _contexto.Mesas
                .AsNoTracking()
                .Where(m => m.Estado == valor)
                .OrderBy(m => m.NumeroMesa)
                .ToList();
        }

        public Mesa? ObtenerPorId(int id)
        {
            return _contexto.Mesas
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == id);
        }

        public void Agregar(Mesa mesa)
        {
            ArgumentNullException.ThrowIfNull(mesa);
            _contexto.Mesas.Add(mesa);
            _contexto.SaveChanges();
        }

        public void Actualizar(Mesa mesa)
        {
            ArgumentNullException.ThrowIfNull(mesa);

            var existente = _contexto.Mesas.FirstOrDefault(m => m.Id == mesa.Id)
                ?? throw new KeyNotFoundException($"No existe una mesa con Id {mesa.Id}.");

            existente.NumeroMesa = mesa.NumeroMesa;
            existente.Estado = mesa.Estado;
            existente.Capacidad = mesa.Capacidad;
            existente.Seccion = mesa.Seccion;
            existente.PrecioPorPersona = mesa.PrecioPorPersona;
            existente.IdentificacionCliente = mesa.IdentificacionCliente;
            existente.NombreCliente = mesa.NombreCliente;
            existente.FechaReserva = mesa.FechaReserva;
            existente.CantidadComensales = mesa.CantidadComensales;
            existente.DepositoDeGarantia = mesa.DepositoDeGarantia;

            _contexto.SaveChanges();
        }
    }
}
