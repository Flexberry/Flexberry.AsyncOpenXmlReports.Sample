using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Microsoft.DependencyInjection;

namespace Flexberry.Quartz.Sample.Service
{
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
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Конфигурация.
            var conf = new ConfigurationBuilder()
                .AddJsonFile("adapterSettings.json", optional: false, reloadOnChange: false)
                .Build();

            // Настройки.
            var adapterStartup = new AdapterStartup(conf);

            // Построение веб-хоста.
            var builder = WebHost.CreateDefaultBuilder()
                // Добавляем конфигурацию.
                .UseConfiguration(conf)
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
            return host.RunAsync();
        }

        /// <summary>
        /// Завершение сервиса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены. Указывает, что процесс завершения был прерван.</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return host.StopAsync();
        }
    }
}
