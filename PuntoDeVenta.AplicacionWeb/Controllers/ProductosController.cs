using Microsoft.AspNetCore.Mvc;

namespace PuntoDeVenta.AplicacionWeb.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index() => View();
    }
}
