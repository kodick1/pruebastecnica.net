using Autofac;
using Autofac.Integration.Mvc;
using SistemaIncidencias.Data;
using SistemaIncidencias.Repositories;
using SistemaIncidencias.Services;
using System.Web.Mvc;

namespace SistemaIncidencias.App_Start
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Registrar controladores
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Registrar contexto de base de datos
            builder.RegisterType<IncidentDbContext>().AsSelf().InstancePerRequest();

            // Registrar repositorios genéricos
            builder.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IRepository<>))
                .InstancePerRequest();

            // Registrar servicios
            builder.RegisterType<IncidentService>()
                .As<IIncidentService>()
                .InstancePerRequest();

            // Construir contenedor
            var container = builder.Build();

            // Configurar resolución de dependencias
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}