using Clientes.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Clientes.Tests.FunctionalTests
{
    public class ClienteTestStartup : Startup
    {
        public ClienteTestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(Configuration);
            base.ConfigureServices(services);
        }

        protected override void ConfigureAuth(IApplicationBuilder app)
        {
            if (Configuration["isTest"] == bool.TrueString.ToLowerInvariant())
            {

            }
            else
            {
                base.ConfigureAuth(app);
            }
        }
    }
}