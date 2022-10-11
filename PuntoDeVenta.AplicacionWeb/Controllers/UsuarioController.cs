using Microsoft.AspNetCore.Mvc;

namespace PuntoDeVenta.AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
