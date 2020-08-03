using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Stef.AuditClient.MicrosoftIdentityClient.Options;

namespace ConsoleAppCallApi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            //string Scope1 = "api://821eb724-edb8-4dba-b425-3f953250c0ae/API.Access";
            //string Scope2 = "821eb724-edb8-4dba-b425-3f953250c0ae/API.Access";
            //string Scope3 = "API.Access";
            var tenant = "020b0cf3-d6b2-464e-9b2d-45e124244428";
            var clientId = "c64feb8e-4545-4f2c-a0dd-6b72a8d1a8bb";
            var clientSecret = "M3QN2z.KH7_M~Hm85SJ_I9Y-4IY0SIGo_~";

            var host = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddAccessTokenManagement(options =>
                    //{
                    //    options.Client.Clients.Add("AzureAD", new ClientCredentialsTokenRequest
                    //    {
                    //        Address = $"https://login.microsoftonline.com/{tenant}/oauth2/token",
                    //        ClientId = clientId,
                    //        ClientSecret = clientSecret,
                    //        GrantType = "client_credentials",

                    //        // Scope = Scope1,

                    //        Parameters =
                    //        {
                    //            { "resource", "821eb724-edb8-4dba-b425-3f953250c0ae" }
                    //        }
                    //    });
                    //});

                    //services.AddClientAccessTokenClient("client", configureClient: client =>
                    //{
                    //    client.BaseAddress = new Uri("https://localhost:5001");
                    //});



                    //services.AddOptions<AuditClientMicrosoftIdentityClientOptions>("AuditClientMicrosoftIdentityClientOptions");
                    //services.Configure<AuditClientMicrosoftIdentityClientOptions>(hostContext.Configuration.GetSection("AuditClientMicrosoftIdentityClientOptions"));

                    //services.AddSingleton<IAuditClientMicrosoftIdentityClient, AuditClientMicrosoftIdentityClient>();

                    //services.Configure<AuditClientMicrosoftIdentityClientOptions>(hostContext.Configuration.GetSection("AuditClientMicrosoftIdentityClientOptions"));
                    services.AddAuditClientMicrosoftIdentity(hostContext.Configuration.GetSection("AuditClientMicrosoftIdentityClientOptions"));

                    services.AddHostedService<Worker3AzureIdentity>();
                });

            await host.Build().RunAsync();
        }
    }
}