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

            string Scope1 = "api://821eb724-edb8-4dba-b425-3f953250c0ae/API.Access";
            string Scope2 = "821eb724-edb8-4dba-b425-3f953250c0ae/API.Access";
            string Scope3 = "API.Access";
            var ten = "020b0cf3-d6b2-464e-9b2d-45e124244428";
            var id = "c64feb8e-4545-4f2c-a0dd-6b72a8d1a8bb";
            var secret = "M3QN2z.KH7_M~Hm85SJ_I9Y-4IY0SIGo_~";

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddAccessTokenManagement(options =>
                    {
                        options.Client.Clients.Add("AzureAD", new ClientCredentialsTokenRequest
                        {
                            Address = $"https://login.microsoftonline.com/{ten}/oauth2/token",
                            ClientId = id,
                            ClientSecret = secret,
                            GrantType = "client_credentials",

                            Scope = Scope1,

                            Parameters =
                            {
                                { "resource", "821eb724-edb8-4dba-b425-3f953250c0ae" }
                            }
                        });
                    });

                    services.AddClientAccessTokenClient("client", configureClient: client =>
                    {
                        client.BaseAddress = new Uri("https://localhost:5001");
                    });

                    services.AddHostedService<Worker>();
                });

            host.Build().Run();
        }
    }
}