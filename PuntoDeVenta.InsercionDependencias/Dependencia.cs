using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*===REFERENCIAS DEL PROYECTO===*/
/* Importing the namespaces. */
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuntoDeVenta.AccesoDatos.DBContext;
using Microsoft.EntityFrameworkCore;
using PuntoDeVenta.AccesoDatos.Implementacion;
using PuntoDeVenta.AccesoDatos.Interfaces;
using IVentaRepository = PuntoDeVenta.AccesoDatos.Implementacion.IVentaRepository;
using VentaRepository = PuntoDeVenta.AccesoDatos.Implementacion.IVentaRepository;
using PuntoDeVenta.LogicaDeNegocio.Implementacion;
using PuntoDeVenta.LogicaDeNegocio.Interfaces;

namespace PuntoDeVenta.InsercionDependencias
{
    public static class Dependencia
    {
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration configuration)
        {
            /* Registrar el DbContext con el contenedor DI. */
            services.AddDbContext<DB_PROYECTOContext>(options =>
            {
                /* Obtención de la cadena de conexión del archivo appsettings.json. */
                options.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();
            services.AddScoped<IRolServicio, RolServicio>();
        }
    }
}
