using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Clientes.Tests.FunctionalTests
{
    public class ClienteScenarioBase
    {
        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ClienteScenarioBase))
                .Location;

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .UseConfiguration(configuration)
                .UseStartup<ClienteTestStartup>();

            var testServer = new TestServer(hostBuilder);

            return testServer;
        }

        public StringContent CreateContent(object obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            return new StringContent(content, UTF8Encoding.UTF8, "application/json");
        }
    }

    public class ClienteEndPoints
    {
        public static string Cadastro = "api/v1/cliente";
    }
}