namespace Flexberry.Quartz.Sample.Service
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Unity.Microsoft.DependencyInjection;

    /// <summary>
    /// Сервис адаптера.
    /// </summary>
    public class AdapterService : IHostedService
    {
        /// <summary>
        /// Веб-хост.
        /// </summary>
        private IWebHost host = null;

        /// <summary>
        /// Запуск сервиса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены. Указывает, что процесс запуска был прерван.</param>
        /// <returns>Task.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Настройки.
            var adapterStartup = new AdapterStartup(Adapter.Configuration);

            // Построение веб-хоста.
            var builder = WebHost.CreateDefaultBuilder()

                // Добавляем конфигурацию.
                .UseConfiguration(Adapter.Configuration)

                // Добавляем IIS.
                .UseIISIntegration()

                // Включаем использование unity.
                .UseUnityServiceProvider(Adapter.Container)

                // Настройка севрисов.
                .ConfigureServices(adapterStartup.ConfigureServices)

                // Общая настройка.
                .Configure(adapterStartup.Configure);

            host = builder.Build();

            // Запуск веб-хоста.
            return host.RunAsync(cancellationToken);
        }

        /// <summary>
        /// Завершение сервиса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены. Указывает, что процесс завершения был прерван.</param>
        /// <returns>Task.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return host.StopAsync(cancellationToken);
        }
    }
}
