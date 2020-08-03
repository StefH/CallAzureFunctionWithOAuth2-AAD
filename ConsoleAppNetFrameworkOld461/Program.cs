using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace ConsoleAppNetFrameworkOld461
{
    class Program
    {
        static void Main(string[] args)
        {
            var tenant = "020b0cf3-d6b2-464e-9b2d-45e124244428";
            var clientId = "c64feb8e-4545-4f2c-a0dd-6b72a8d1a8bb";
            var clientSecret = "M3QN2z.KH7_M~Hm85SJ_I9Y-4IY0SIGo_~";

            UnityContainer unityContainer = new UnityContainer();


            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
            // services.AddDistributedMemoryCache();
            services.AddAuditClientMicrosoftIdentity(o =>
            {
                o.BaseAddress = new System.Uri("https://localhost:5001");
                o.TenantId = tenant;
                o.ClientId = clientId;
                o.ClientSecret = clientSecret;
                o.Resource = "821eb724-edb8-4dba-b425-3f953250c0ae";
            });

            services.BuildServiceProvider(unityContainer);

            var sw = unityContainer.Resolve<Worker>();
            sw.ExecuteAsync(CancellationToken.None).Wait();
        }
    }
}
