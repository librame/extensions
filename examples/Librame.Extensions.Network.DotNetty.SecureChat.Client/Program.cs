using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.DotNetty.SecureChat.Client
{
    using Demo;

    class Program
    {
        static void Main(string[] args)
        {
            var client = DotNettyServiceProvider.Current.GetRequiredService<ISecureChatClient>();
            client.StartAsync(async channel =>
            {
                for (; ;)
                {
                    string msg = Console.ReadLine();
                    if (string.IsNullOrEmpty(msg))
                    {
                        continue;
                    }

                    try
                    {
                        await channel.WriteAndFlushAsync(msg + "\r\n");
                    }
                    catch
                    {
                    }

                    var clientOptions = client.Options.SecureChatClient;
                    if (clientOptions.ExitCommand.Equals(msg, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                await channel.CloseAsync();
            })
            .Wait();
        }
    }
}
