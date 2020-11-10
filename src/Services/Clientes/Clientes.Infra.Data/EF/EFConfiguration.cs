using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Clientes.Infra.Data.EF
{
    public static class EFConfiguration
    {
        public static IServiceCollection InitializeEFConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ApplicationContextDb>(options =>
                {
                    options.UseSqlServer(connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            //sqlOptions.MigrationsAssembly("Clientes.Infra.Data");
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                },
                ServiceLifetime.Scoped
            );

            return services;
        }
    }
}
