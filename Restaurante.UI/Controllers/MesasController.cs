using Microsoft.AspNetCore.Mvc;
using Restaurante.BL;
using Restaurante.Model;

namespace Restaurante.UI.Controllers
{
    /// <summary>
    /// Módulo de Registro de mesas: listar, filtrar, agregar y editar.
    /// El controlador solo orquesta llamadas al servicio BL; no contiene
    /// lógica de negocio ni cálculos.
    /// </summary>
    public class MesasController : Controller
    {
        private readonly IMesaService _servicio;

        public MesasController(IMesaService servicio)
        {
            _servicio = servicio;
        }

        public IActionResult Index(string? busqueda)
        {
            var mesas = _servicio.ObtenerTodas(busqueda);
            ViewBag.Busqueda = busqueda;
            return View(mesas);
        }

        public IActionResult Crear() => View(new Mesa());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Mesa mesa)
        {
            if (!ModelState.IsValid)
            {
                return View(mesa);
            }

            _servicio.RegistrarMesa(mesa);
            TempData["Mensaje"] = $"Mesa {mesa.NumeroMesa} registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
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
        public IActionResult Editar(Mesa mesa)
        {
            if (!ModelState.IsValid)
            {
                return View(mesa);
            }

            try
            {
                _servicio.EditarMesa(mesa);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            TempData["Mensaje"] = $"Mesa {mesa.NumeroMesa} actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
