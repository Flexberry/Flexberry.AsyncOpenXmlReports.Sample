﻿namespace Flexberry.Quartz.Sample.Service
{
    using System;
    using Flexberry.Quartz.Sample.Service.Jobs;
    using ICSSoft.STORMNET;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Unity;
    using Unity.Microsoft.DependencyInjection;

    /// <summary>
    /// Адаптер для запуска хоста процессов, в котором будет веб-хост для получения запросов на формирование отчетов.
    /// </summary>
    public class Adapter
    {
        /// <summary>
        /// Главный контейнер зависимостей.
        /// </summary>
        public static readonly IUnityContainer Container = ICSSoft.Services.UnityFactory.GetContainer();

        /// <summary>
        /// Конфигурация.
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Главный хост.
        /// </summary>
        private IHost host;

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
        /// Закрытие хоста.
        /// </summary>
        /// <param name="serviceHost">Хост процессов.</param>
        protected static void Close(ref IHost serviceHost)
        {
            if (serviceHost != null)
            {
                serviceHost.StopAsync().Wait();
                serviceHost = null;
            }
        }

        /// <summary>
        /// Старт адаптера.
        /// </summary>
        protected void Start()
        {
            Stop();

            // Конфигурация.
            InitConfiguration();

            // Настройки адаптера.
            var adapterStartup = new AdapterStartup(Configuration);

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
        /// Инициализация конфигурации.
        /// </summary>
        private void InitConfiguration()
        {
            // Конфигурация.
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("adapterSettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            LogConfigurationValue("DefConnStr");
            LogConfigurationValue("SecurityConnString");
            LogConfigurationValue(JobTools.UploadUrlConfigParamName);
            LogConfigurationValue(JobTools.TemplatesPathConfigParamName);
            LogConfigurationValue(JobTools.BackendRootConfigParamName);
        }

        /// <summary>
        /// Инициализация свойства конфигурации.
        /// </summary>
        /// <param name="paramName">Имя параметра.</param>
        private void LogConfigurationValue(string paramName)
        {
            LogService.Log.Debug($"Param: {paramName} = {Configuration[paramName]}");
        }
    }
}
