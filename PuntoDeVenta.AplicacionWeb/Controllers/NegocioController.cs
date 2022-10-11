using Microsoft.AspNetCore.Mvc;

namespace PuntoDeVenta.AplicacionWeb.Controllers
{
    public class NegocioController : Controller
    {
        public IActionResult Index() => View();
    }
}
