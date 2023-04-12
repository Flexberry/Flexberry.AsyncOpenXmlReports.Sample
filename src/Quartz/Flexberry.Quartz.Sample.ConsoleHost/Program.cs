namespace Flexberry.Quartz.Sample.ConsoleHost
{
    using System;
    using Flexberry.Quartz.Sample.Service;

    /// <summary>
    /// Программа.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            var adapter = new Adapter();
            adapter.OnStart();
            Console.WriteLine("Adapter service host is started. Press any key to stop it and exit . . .");
        }
    }
}
