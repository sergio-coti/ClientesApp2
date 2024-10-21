using ClientesApp.Application.Interfaces.Messages;
using ClientesApp.Infra.Messages.Publishers;
using ClientesApp.Infra.Messages.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientesApp.Infra.Messages.Extensions
{
    public static class RabbitMQExtension
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            //capturar as configurações do /appsettings
            var rabbitMQSettings = new RabbitMQSettings();
            new ConfigureFromConfigurationOptions<RabbitMQSettings>
                (configuration.GetSection("RabbitMQSettings"))
                .Configure(rabbitMQSettings);

            //injeção de dependência
            services.AddSingleton(rabbitMQSettings);
            services.AddTransient<IMessagePublisher, MessagePublisher>();

            return services;
        }
    }
}
