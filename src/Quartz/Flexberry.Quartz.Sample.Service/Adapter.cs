using ICSSoft.Services;
using ICSSoft.STORMNET;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace Flexberry.Quartz.Sample.Service
{
    /// <summary>
    /// Адаптер для запуска хоста процессов, в котором будет веб-хост для получения запросов на формирование отчетов.
    /// </summary>
    public class Adapter
    {
        /// <summary>
        /// Главный хост.
        /// </summary>
        private IHost host = null;

        /// <summary>
        /// Главный контейнер зависимостей.
        /// </summary>
        public static readonly IUnityContainer Container = UnityFactory.GetContainer();

        /// <summary>
        /// Старт адаптера.
        /// </summary>
        public void OnStart()
        {
            try
            {
                LogService.LogDebug("Starting the adapter service . . .");

                Start();

                LogService.LogInfo("The adapter service is started.");
            }
            catch (Exception ex)
            {
                if (LogService.Log.IsFatalEnabled)
                {
                    LogService.Log.Fatal("The adapter service start fault:", ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Завершение работы адаптера.
        /// </summary>
        public void OnStop()
        {
            try
            {
                LogService.LogDebug("Stopping the adapter service . . .");

                Stop();

                LogService.LogInfo("The adapter service is stopped.");
            }
            catch (Exception ex)
            {
                if (LogService.Log.IsFatalEnabled)
                {
                    LogService.Log.Fatal("The adapter service stop fault:", ex);
                }

                throw;
            }

        }

        /// <summary>
        /// Старт адаптера.
        /// </summary>
        protected void Start()
        {
            Stop();

            // Конфигурация.
            var conf = new ConfigurationBuilder()
                .AddJsonFile("adapterSettings.json", optional: false, reloadOnChange: false)
                .Build();

            // Настройки адаптера.
            var adapterStartup = new AdapterStartup(conf);

            // Построение хоста.
            var builder = Host.CreateDefaultBuilder()
                // Добавляем файл конфигурации.
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddJsonFile("adapterSettings.json");
                })
                // Включаем использование unity.
                .UseUnityServiceProvider(Container)
                // Настраиваем контейнер.
                .ConfigureContainer<IUnityContainer>(adapterStartup.ConfigureContainer)
                // Настраиваем сервисы.
                .ConfigureServices((hostContext, services) =>
                {
                    // Добавляем сервис адаптера.
                    services.AddHostedService<AdapterService>();
                });

            host = builder.Build();

            // Запускаем хост.
            host.Run();
        }

        /// <summary>
        /// Завершение работы адаптера.
        /// </summary>
        protected void Stop()
        {
            Close(ref host);
        }

        /// <summary>
        /// Закрытие хоста.
        /// </summary>
        /// <param name="serviceHost">Хост процессов.</param>
        protected void Close(ref IHost serviceHost)
        {
            if (serviceHost != null)
            {
                serviceHost.StopAsync().Wait();
                serviceHost = null;
            }
        }
    }
}
