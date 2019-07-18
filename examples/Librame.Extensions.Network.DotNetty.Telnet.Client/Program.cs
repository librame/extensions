using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.Telnet.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<ITelnetClient>();

            client.StartAsync(async channel =>
            {
                for (; ; )
                {
                    string line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    try
                    {
                        await channel.WriteAndFlushAsync(line + "\r\n");
                    }
                    catch
                    {
                    }

                    var clientOptions = client.Options.SecureChatClient;
                    if (string.Equals(line, clientOptions.ExitCommand, StringComparison.OrdinalIgnoreCase))
                    {
                        await channel.CloseAsync();
                        break;
                    }
                }
            })
            .Wait();
        }
    }
}
