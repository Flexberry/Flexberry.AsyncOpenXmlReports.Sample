using ICSSoft.STORMNET;
using ICSSoft.STORMNET.Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Flexberry.Quartz.Sample.Service
{
    public class AdapterStartup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterStartup" /> class.
        /// </summary>
        /// <param name="configuration">An application configuration properties.</param>
        public AdapterStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// An application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configurate application services.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </remarks>
        /// <param name="services">An collection of application services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            string connStr = Configuration["DefConnStr"];

            LogService.LogDebug("Adapter.ConfigureServices Start");

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddScoped<IUserWithRoles, AdapterUserService>();
            services.AddTransient<IDataService, PostgresDataService>((serviceProvider) =>
            {
                return new PostgresDataService() { CustomizationString = connStr };
            });

            services.AddCors();

            LogService.LogDebug("Adapter.ConfigureServices End");
        }

        /// <summary>
        /// Configurate the HTTP request pipeline.
        /// </summary>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </remarks>
        /// <param name="app">An application configurator.</param>
        /// <param name="env">Information about web hosting environment.</param>
        public void Configure(IApplicationBuilder app)
        {
            LogService.LogDebug("Adapter.Configure Start");

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMvc(routes =>
            {
                routes.MapRoute("Quartz", "api/quartz", defaults: new { controller = "Quartz", action = "TestReport" });
            });

            LogService.LogDebug("Adapter.Configure End");
        }
    }
}
