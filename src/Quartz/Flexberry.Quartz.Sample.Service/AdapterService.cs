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
    public class AdapterService : IHostedService
    {
        private IWebHost host = null;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new ConfigurationBuilder()
                .AddJsonFile("adapterSettings.json", optional: false, reloadOnChange: false)
                .Build();

            var adapterStartup = new AdapterStartup(conf);

            var builder =
                WebHost.CreateDefaultBuilder()
                    .UseConfiguration(conf)
                    .UseIISIntegration()
                    .UseUnityServiceProvider(Adapter.Container)
                    .ConfigureServices(adapterStartup.ConfigureServices)
                    .Configure(adapterStartup.Configure);

            host = builder.Build();

            return host.RunAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return host.StopAsync();
        }
    }
}
