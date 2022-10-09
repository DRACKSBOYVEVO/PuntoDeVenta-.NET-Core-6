using Microsoft.AspNetCore.Mvc;

namespace PuntoDeVenta.AplicacionWeb.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
