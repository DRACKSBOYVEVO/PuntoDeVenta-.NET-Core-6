using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PuntoDeVenta.AplicacionWeb.Controllers
{
    public class UsuariosController : Controller
    {
        /*inyeccion de servicios*/

        /*Loggear al usuario*/
        private readonly SignInManager<IdentityUser> signInManager;
        /*Crear al usuario*/
        private readonly UserManager<IdentityUser> userManager;

        public UsuariosController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        /*Acción de logín
         por si el usuario tiene un error por parte de microsoft y queremos mostrarlo
         */
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }
            return View();
        }

        /*Accion que redirige a los usuarios a los servicio de Microsoft*/

        /*
         * PARAMETROS RECIBIDOS POR LoginExterno
         proveedor: Facebook, Google, Microsoft. Etc.
         urlRetorno: por si quiero devolver al usuario donde se encontraba
         */
        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string? urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            /*usamos el signInManager configurar las propiedades de autenticacion*/
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            /*hacemos que el usuario sea redirigido a los servidores del proveedor de identidad (Microsft)*/
            return new ChallengeResult(proveedor, propiedades);
        }

        /*====LOGICA PARA CONECTAR CON CUALQUIER PROVEEDOR DE IDENTIFICACION EXTERNO====*/
        /*Registrandolo en el sistema si no tiene una cuenta, si no, ingrese con esa cuenta de Microsoft que ya tiene creada*/
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string? urlRetorno = null, string? remoteError = null)
        {
            urlRetorno ??= Url.Content("~/");
            var mensaje = "";

            if (remoteError != null)
            {
                mensaje = $"Error from external provider: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            /*Capturando una excepcion por si sucede*/
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                mensaje = "Error loading external login Information.";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            //Si ya existe una cuenta creada
            if (resultadoLoginExterno.Succeeded)
            {
                return LocalRedirect(urlRetorno);
            }

            string email = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = "Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var usuario = new IdentityUser() { Email = email, UserName = email };

            var resultadoCrearUsuario = await userManager.CreateAsync(usuario);
            if (!resultadoCrearUsuario.Succeeded)
            {
                mensaje = resultadoCrearUsuario.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoAgregarLogin = await userManager.AddLoginAsync(usuario, info);
            if (resultadoAgregarLogin.Succeeded)
            {
                await signInManager.SignInAsync(usuario, isPersistent: false, info.LoginProvider);
                return LocalRedirect(urlRetorno);
            }

            mensaje = "Ha ocurrido un error agregando el login.";
            return RedirectToAction("login", routeValues: new { mensaje });
        }

        /*Logica del Loguout*/
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            /*Se borran las cookies del usuario*/
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
