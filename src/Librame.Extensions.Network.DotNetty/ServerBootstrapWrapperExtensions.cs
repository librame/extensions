#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs;
using DotNetty.Codecs.Http;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 服务器引导程序封装器静态扩展。
    /// </summary>
    public static class ServerBootstrapWrapperExtensions
    {
        /// <summary>
        /// 增加 HTTP 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="httpHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddHttpHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler httpHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast("encoder", new HttpResponseEncoder());
                pipeline.AddLast("decoder", new HttpRequestDecoder(4096, 8192, 8192, false));
                pipeline.AddLast(httpHandler);
            });
        }

        /// <summary>
        /// 增加 Discard 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="discardHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddDiscardHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler discardHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new LoggingHandler("CONN"));
                pipeline.AddLast(discardHandler);
            });
        }

        /// <summary>
        /// 增加 Echo 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="echoHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddEchoHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler echoHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                pipeline.AddLast("echo", echoHandler);
            },
            addTlsPipelineName: true);
        }

        /// <summary>
        /// 增加 Factorial 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="factorialHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddFactorialHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler factorialHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new LoggingHandler("CONN"));
                pipeline.AddLast(new BigIntegerEncoder());
                pipeline.AddLast(new BigIntegerDecoder());
                pipeline.AddLast(factorialHandler);
            });
        }

        /// <summary>
        /// 增加 SecureChat 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="secureChatHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddSecureChatHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler secureChatHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                pipeline.AddLast(new StringEncoder());
                pipeline.AddLast(new StringDecoder());
                pipeline.AddLast(secureChatHandler);
            });
        }

        /// <summary>
        /// 增加 Telnet 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="telnetHandler">给定的 <see cref="IChannelHandler"/> 工厂方法。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddTelnetHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler telnetHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                pipeline.AddLast(new StringEncoder());
                pipeline.AddLast(new StringDecoder());
                pipeline.AddLast(telnetHandler);
            });
        }

        /// <summary>
        /// 增加 WebSocket 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IServerBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="webSocketHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IServerBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IServerBootstrapWrapper AddWebSocketHandler<TChannelHandler>(this IServerBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler webSocketHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandler<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new HttpServerCodec());
                pipeline.AddLast(new HttpObjectAggregator(65536));
                pipeline.AddLast(webSocketHandler);
            });
        }

    }
}
