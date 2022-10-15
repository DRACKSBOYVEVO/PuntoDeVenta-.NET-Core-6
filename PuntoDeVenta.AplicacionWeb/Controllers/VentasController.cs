using Microsoft.AspNetCore.Mvc;

namespace PuntoDeVenta.AplicacionWeb.Controllers
{
    public class VentasController : Controller
    {
        public IActionResult NuevaVenta() => View();

        public IActionResult HistorialVenta() => View();
    }
}
