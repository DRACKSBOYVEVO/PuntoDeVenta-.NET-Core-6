/* Importar los espacios de nombres de las clases que he creado. */
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PuntoDeVenta.AccesoDatos;
using PuntoDeVenta.AplicacionWeb;
using PuntoDeVenta.InsercionDependencias;

/* Creando un objeto constructor. */
var builder = WebApplication.CreateBuilder(args);

/* Añadiendo los controladores y las vistas a la aplicación. */
builder.Services.AddControllersWithViews();

/* Un método que he creado para inyectar las dependencias. */
builder.Services.InyectarDependencia(builder.Configuration);

/*Configuración de Login Externo de Azure*/

/*Congiracion de los datos agregados en app.json*/
builder.Services.AddAuthentication().AddMicrosoftAccount(opciones => { opciones.ClientId = builder.Configuration["MicrosoftClientId"]!; opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"]!;});

builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlServer("name=CadenaSQL"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones => { opciones.SignIn.RequireConfirmedAccount = false; }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones => { opciones.LoginPath = "/usuarios/login"; opciones.AccessDeniedPath = "/usuarios/login";});

/*Configuración de Login Externo de Azure*/

/* Creando una instancia de la clase `WebApplication`. */
var app = builder.Build();

/* Comprobación de si la aplicación está en modo de desarrollo. 
 * Si no lo está, entonces usará el manejador de de excepción. */
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

/*Azure*/

app.UseAuthorization();

app.UseAuthorization();

/* Mapeo de la ruta del controlador. */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LoginExterno}/{action=Index}/{id?}");

app.Run();
