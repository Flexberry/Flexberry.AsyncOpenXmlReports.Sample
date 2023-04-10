using ICSSoft.STORMNET;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Flexberry.Quartz.Sample.Service
{
    public class Adapter
    {
        private IWebHost _webHost = null;
        //private IHost _host = null;

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

            var builder =
                WebHost.CreateDefaultBuilder()
                    .UseConfiguration(conf)
                    //.ConfigureAppConfiguration((hostingContext, config) =>
                    //    {
                    //        config.AddJsonFile("adapterSettings.json", optional: false, reloadOnChange: false);
                    //    })
                    .UseIISIntegration()
                    //.UseStartup<AdapterStartup>();
                    .ConfigureServices(adapterStartup.ConfigureServices)
                    .Configure(adapterStartup.Configure);

            _webHost = builder.Build();
            _webHost.Run();
        }

        protected void Stop()
        {
            Close(ref _webHost);
        }

        protected void Close(ref IWebHost serviceHost)
        {
            if (serviceHost != null)
            {
                serviceHost.StopAsync().Wait();
                serviceHost = null;
            }
        }
    }
}
