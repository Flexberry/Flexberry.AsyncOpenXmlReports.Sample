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
    public class Adapter
    {
        private IHost host = null;

        public static readonly IUnityContainer Container = UnityFactory.GetContainer();

        public Adapter()
        {

        }

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

        protected void Start()
        {
            Stop();

            var conf = new ConfigurationBuilder()
                .AddJsonFile("adapterSettings.json", optional: false, reloadOnChange: false)
                .Build();

            var adapterStartup = new AdapterStartup(conf);

            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(cfg =>
                {
                    cfg.AddJsonFile("adapterSettings.json");
                })
                .UseUnityServiceProvider(Container)
                .ConfigureContainer<IUnityContainer>(adapterStartup.ConfigureContainer)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<AdapterService>();
                });

            host = builder.Build();
            host.Run();
        }

        protected void Stop()
        {
            Close(ref host);
        }

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
