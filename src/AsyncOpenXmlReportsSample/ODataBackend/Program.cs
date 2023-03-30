namespace IIS.AsyncOpenXmlReportsSample
{
    using ICSSoft.Services;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Unity;
    using Unity.Microsoft.DependencyInjection;

    /// <summary>
    /// Основной класс приложения.
    /// </summary>
    public static class Program
    {
        private static readonly IUnityContainer Container = UnityFactory.GetContainer();

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы запуска.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Создать инициализатор приложения.
        /// </summary>
        /// <param name="args">Аргументы запуска.</param>
        /// <returns>Инициализатор приложения.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseUnityServiceProvider(Container)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
