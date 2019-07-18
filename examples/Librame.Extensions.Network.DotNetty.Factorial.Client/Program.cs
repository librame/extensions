using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Factorial.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<IFactorialClient>();

            client.StartAsync(channel =>
            {
                Console.ReadLine();
            })
            .Wait();
        }
    }
}
