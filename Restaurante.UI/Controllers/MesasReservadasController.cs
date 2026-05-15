using Microsoft.AspNetCore.Mvc;
using Restaurante.BL;

namespace Restaurante.UI.Controllers
{
    /// <summary>
    /// Módulo de Mesas Reservadas: listar, filtrar por nombre del cliente,
    /// ver el detalle completo y liberar la mesa (estado vuelve a Disponible).
    /// </summary>
    public class MesasReservadasController : Controller
    {
        private readonly IMesaService _servicio;

        public MesasReservadasController(IMesaService servicio)
        {
            _servicio = servicio;
        }

        public IActionResult Index(string? busqueda)
        {
            var mesas = _servicio.ObtenerReservadas(busqueda);
            ViewBag.Busqueda = busqueda;
            return View(mesas);
        }

        public IActionResult Detalle(int id)
        {
            var mesa = _servicio.ObtenerPorId(id);
            if (mesa is null)
            {
                return NotFound();
            }
            return View(mesa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Liberar(int id)
        {
            try
            {
                _servicio.LiberarMesa(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            TempData["Mensaje"] = "La mesa quedó disponible nuevamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
