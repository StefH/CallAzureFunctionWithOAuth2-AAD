using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            

            services.AddAuthorization(o =>
            {
                //o.AddPolicy("default", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    // policy.RequireClaim("http://schemas.microsoft.com/identity/claims/scope", "API.Access");
                //});

                o.AddPolicy("default", builder =>
                {
                    builder
                        .RequireAuthenticatedUser()
                        .RequireClaim("http://schemas.microsoft.com/identity/claims/scope", new[] { "user_impersonation", "API.Access" });
                });


                //o.AddPolicy(Policies.WriteTodoItems, policy =>
                //{
                //    policy.RequirePermissions(
                //        delegated: new[] { Scopes.TodosReadWrite },
                //        application: new[] { AppRoles.TodosReadWrite });
                //});


            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
