/* Importar los espacios de nombres de las clases que he creado. */
using PuntoDeVenta.AccesoDatos;
using PuntoDeVenta.InsercionDependencias;

/* Creando un objeto constructor. */
var builder = WebApplication.CreateBuilder(args);

/* A�adiendo los controladores y las vistas a la aplicaci�n. */
builder.Services.AddControllersWithViews();

/* Un m�todo que he creado para inyectar las dependencias. */
builder.Services.InyectarDependencia(builder.Configuration);

/* Creando una instancia de la clase `WebApplication`. */
var app = builder.Build();

/* Comprobaci�n de si la aplicaci�n est� en modo de desarrollo. 
 * Si no lo est�, entonces usar� el manejador de de excepci�n. */
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

/* Mapeo de la ruta del controlador. */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
