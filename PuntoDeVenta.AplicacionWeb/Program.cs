/* Importar los espacios de nombres de las clases que he creado. */
using PuntoDeVenta.AccesoDatos;
using PuntoDeVenta.InsercionDependencias;

/* Creando un objeto constructor. */
var builder = WebApplication.CreateBuilder(args);

/* Añadiendo los controladores y las vistas a la aplicación. */
builder.Services.AddControllersWithViews();

/* Un método que he creado para inyectar las dependencias. */
builder.Services.InyectarDependencia(builder.Configuration);

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

app.UseAuthorization();

/* Mapeo de la ruta del controlador. */
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
