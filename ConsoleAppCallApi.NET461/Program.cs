using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace ConsoleAppCallFunctionWithAAD
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //string Scope1 = "api://821eb724-edb8-4dba-b425-3f953250c0ae/API.Access";
            //string Scope2 = "821eb724-edb8-4dba-b425-3f953250c0ae/API.Access";
            //string Scope3 = "API.Access";
            var tenant = "020b0cf3-d6b2-464e-9b2d-45e124244428";
            var clientId = "c64feb8e-4545-4f2c-a0dd-6b72a8d1a8bb";
            var clientSecret = "M3QN2z.KH7_M~Hm85SJ_I9Y-4IY0SIGo_~";

            UnityContainer unityContainer = new UnityContainer();
           

            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
            services.AddDistributedMemoryCache();
            services.AddAuditClient(o =>
            {
                /*
                 * "AuditClientOptions": {
    "TenantId": "020b0cf3-d6b2-464e-9b2d-45e124244428",
    "ClientId": "c64feb8e-4545-4f2c-a0dd-6b72a8d1a8bb",
    "ClientSecret": "M3QN2z.KH7_M~Hm85SJ_I9Y-4IY0SIGo_~",
    "Resource": "821eb724-edb8-4dba-b425-3f953250c0ae",
    "BaseAddress": "https://localhost:5001"
  }*/
                o.BaseAddress = "https://localhost:5001";
                o.TenantId = tenant;
                o.ClientId = clientId;
                o.ClientSecret = clientSecret;
                o.Resource = "821eb724-edb8-4dba-b425-3f953250c0ae";
            });

            //services.AddHostedService<Worker2>();
            services.BuildServiceProvider(unityContainer);

            //var host = Host.CreateDefaultBuilder(args)
            //    .UseSerilog()
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddAuditClient(hostContext.Configuration.GetSection("AuditClientOptions"));

            //        services.AddHostedService<Worker2>();
            //    });

            //host.Build().Run();

            var sw = unityContainer.Resolve<Worker2>();
            sw.ExecuteAsync(CancellationToken.None).Wait();
        }
    }
}