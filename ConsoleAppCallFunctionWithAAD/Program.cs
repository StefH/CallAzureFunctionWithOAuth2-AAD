using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace ConsoleAppCallFunctionWithAAD
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var tenantId = "020b0cf3-d6b2-464e-9b2d-45e124244428";

            // Dit is FunctionsClient2 (werkt)
            var clientId1 = "9995ef6f-28cf-4752-b0af-97bf5d508094";
            var clientSecret1 = "UFuQh8i.-dJ3PuQ_7xCjLYqm6~_3K0sY3b";

            // Dit is FunctionsClient (werkt)
            var clientId = "61de2531-d4d5-4c54-9386-4d87a6f2c2a7";
            var clientSecret = "-e-fhGQ-9ehMkcb43tEYv_1EecW.EXOEs1";

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAccessTokenManagement(options =>
                    {
                        options.Client.Clients.Add("AzureAD", new ClientCredentialsTokenRequest
                        {
                            Address = $"https://login.microsoftonline.com/{tenantId}/oauth2/token",
                            ClientId = clientId,
                            ClientSecret = clientSecret,
                            GrantType = "client_credentials",

                            Parameters =
                            {
                                { "resource", "https://stef-function.azurewebsites.net"}
                            }
                        });
                    });

                    services.AddClientAccessTokenClient("client", configureClient: client =>
                    {
                        client.BaseAddress = new Uri("https://stef-function.azurewebsites.net/api/");
                    });

                    services.AddHostedService<Worker>();
                });

            host.Build().Run();
        }
    }
}