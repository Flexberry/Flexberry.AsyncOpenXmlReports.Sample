namespace Flexberry.Quartz.Sample.Service
{
    using System;
    using ICSSoft.STORMNET;
    using ICSSoft.STORMNET.Business;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Unity;

    /// <summary>
    /// Настройки для запуска хоста адаптера.
    /// </summary>
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
            LogService.LogDebug("Adapter.ConfigureServices Start");

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
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
        public void Configure(IApplicationBuilder app)
        {
            LogService.LogDebug("Adapter.Configure Start");

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMvc(routes =>
            {
                routes.MapRoute("Quartz", "api/quartz", defaults: new { controller = "Quartz", action = "SampleReport" });
            });

            LogService.LogDebug("Adapter.Configure End");
        }

        /// <summary>
        /// Configurate application container.
        /// </summary>
        /// <param name="container">Container to configure.</param>
        public void ConfigureContainer(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // FYI: сервисы, в т.ч. контроллеры, создаются из дочернего контейнера.
            while (container.Parent != null)
            {
                container = container.Parent;
            }

            string connStr = Configuration["DefConnStr"];

            if (string.IsNullOrEmpty(connStr))
            {
                throw new System.Configuration.ConfigurationErrorsException("DefConnStr is not specified in Configuration or enviromnent variables.");
            }

            container.RegisterInstance(Configuration);

            container.RegisterFactory<IUserWithRoles>((cont) => new AdapterUserService(), FactoryLifetime.PerThread);

            IDataService mainDataService = new PostgresDataService()
            {
                CustomizationString = connStr,
            };

            container.RegisterInstance<IDataService>(mainDataService, InstanceLifetime.Singleton);
        }
    }
}
