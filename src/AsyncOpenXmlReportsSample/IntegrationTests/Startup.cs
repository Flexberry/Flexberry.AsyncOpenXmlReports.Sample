namespace IIS.AsyncOpenXmlReportsSample
{
    using ICSSoft.STORMNET.Business;
    using ICSSoft.STORMNET.Security;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureHostConfiguration(builder => { builder.AddJsonFile("appsettings.json"); })
                .ConfigureAppConfiguration((context, builder) => { });

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            string connStr = context.Configuration["DefConnStr"];
            services.AddSingleton<ISecurityManager, EmptySecurityManager>();
            services.AddSingleton<IDataService, PostgresDataService>(f => new PostgresDataService(f.GetService<ISecurityManager>()) { CustomizationString = connStr });
        }
    }
}
