using Microsoft.AspNetCore.Mvc;
using Restaurante.BL;
using Restaurante.BL.Modelos;
using Restaurante.UI.Models;

namespace Restaurante.UI.Controllers
{
    /// <summary>
    /// Módulo de Mesas Disponibles: listar las mesas en estado Disponible (1),
    /// filtrar por número y reservar (con cálculo del depósito en BL).
    /// </summary>
    public class MesasDisponiblesController : Controller
    {
        private readonly IMesaService _servicio;

        public MesasDisponiblesController(IMesaService servicio)
        {
            _servicio = servicio;
        }

        public IActionResult Index(string? busqueda)
        {
            var mesas = _servicio.ObtenerDisponibles(busqueda);
            ViewBag.Busqueda = busqueda;
            return View(mesas);
        }

        public IActionResult Reservar(int id)
        {
            var mesa = _servicio.ObtenerPorId(id);
            if (mesa is null)
            {
                return NotFound();
            }

            var modelo = new ReservaViewModel
            {
                MesaId = mesa.Id,
                NumeroMesa = mesa.NumeroMesa,
                PrecioPorPersona = mesa.PrecioPorPersona,
                FechaReserva = DateTime.Today
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reservar(ReservaViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            try
            {
                _servicio.ReservarMesa(new DatosReserva
                {
                    MesaId = modelo.MesaId,
                    IdentificacionCliente = modelo.IdentificacionCliente,
                    NombreCliente = modelo.NombreCliente,
                    FechaReserva = modelo.FechaReserva,
                    CantidadComensales = modelo.CantidadComensales
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(modelo);
            }

            TempData["Mensaje"] = $"Mesa {modelo.NumeroMesa} reservada para {modelo.NombreCliente}.";
            return RedirectToAction("Index", "MesasReservadas");
        }
    }
}
