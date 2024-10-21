using ClientesApp.Application.Interfaces.Logs;
using ClientesApp.Infra.Data.MongoDB.Contexts;
using ClientesApp.Infra.Data.MongoDB.Settings;
using ClientesApp.Infra.Data.MongoDB.Storages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Data.MongoDB.Extensions
{
    public static class MongoDBExtension
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            //capturar as configurações do /appsettings
            var mongoDBSettings = new MongoDBSettings();
            new ConfigureFromConfigurationOptions<MongoDBSettings>
                (configuration.GetSection("MongoDBSettings"))
                .Configure(mongoDBSettings);

            //injeção de dependência
            services.AddSingleton(mongoDBSettings);
            services.AddScoped<MongoDBContext>();
            services.AddTransient<ILogClienteDataStore, LogClienteDataStore>();

            return services;
        }
    }
}
