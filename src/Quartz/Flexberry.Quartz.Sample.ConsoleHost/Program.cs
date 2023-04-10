using Flexberry.Quartz.Sample.Service;
using System;

namespace Flexberry.Quartz.Sample.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var adapter = new Adapter();
            adapter.OnStart();
            Console.WriteLine("Adapter service host is started. Press any key to stop it and exit . . .");

            Console.ReadKey(true);
            adapter.OnStop();
        }
    }
}
