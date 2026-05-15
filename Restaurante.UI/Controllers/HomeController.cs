using Microsoft.AspNetCore.Mvc;
using Restaurante.UI.Models;
using System.Diagnostics;

namespace Restaurante.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // El home dirige al módulo principal de gestión de mesas.
            return RedirectToAction("Index", "Mesas");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
