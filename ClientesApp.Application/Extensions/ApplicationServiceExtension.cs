using ClientesApp.Application.Interfaces.Applications;
using ClientesApp.Application.Mappings;
using ClientesApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            //injeção de dependência para o AutoMapper
            services.AddAutoMapper(typeof(ClienteProfileMap));

            //injeção de dependência para o MediatR
            services.AddMediatR(m => m.RegisterServicesFromAssemblies
                (AppDomain.CurrentDomain.GetAssemblies()));

            //injeção de dependência para os serviços de aplicação
            services.AddTransient<IClienteAppService, ClienteAppService>();

            return services;
        }
    }
}
