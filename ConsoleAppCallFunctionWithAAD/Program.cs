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

            var id = "61de2531-d4d5-4c54-9386-4d87a6f2c2a7";
            var ten = "020b0cf3-d6b2-464e-9b2d-45e124244428";
            var secret = "-e-fhGQ-9ehMkcb43tEYv_1EecW.EXOEs1";

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAccessTokenManagement(options =>
                    {
                        options.Client.Clients.Add("AzureAD", new ClientCredentialsTokenRequest
                        {
                            Address = "https://login.microsoftonline.com/020b0cf3-d6b2-464e-9b2d-45e124244428/oauth2/token",
                            ClientId = id,
                            ClientSecret = secret,
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