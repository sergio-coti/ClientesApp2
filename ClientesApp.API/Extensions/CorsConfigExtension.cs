﻿namespace ClientesApp.API.Extensions
{
    /// <summary>
    /// Classe de extensão para configuração do Cors
    /// </summary>
    public static class CorsConfigExtension
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //lendo os endereços mapeados no appsettings.json
            var origins = configuration.GetSection("CorsConfigSettings:Origins").Get<string[]>();

            services.AddCors(options => 
            {
                options.AddPolicy("ClientesAppPolicy", builder =>
                {
                    builder.WithOrigins(origins)
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsConfig(this IApplicationBuilder app)
        {
            app.UseCors("ClientesAppPolicy");
            return app;
        }
    }
}
