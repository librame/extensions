using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.SecureChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = DotNettyServiceProvider.Current.GetRequiredService<ISecureChatServer>();
            server.StartAsync(async channel =>
            {
                Console.ReadLine();

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
