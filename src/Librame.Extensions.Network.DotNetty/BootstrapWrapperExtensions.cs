#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets.Extensions.Compression;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 引导程序封装器静态扩展。
    /// </summary>
    public static class BootstrapWrapperExtensions
    {
        /// <summary>
        /// 增加 Discard 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddDiscardHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<ISocketChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new LoggingHandler());
                pipeline.AddLast(channelHandler);
            });
        }

        /// <summary>
        /// 增加 Echo 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddEchoHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<ISocketChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new LoggingHandler());
                pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                pipeline.AddLast("echo", channelHandler);
            },
            addTlsPipelineName: true);
        }

        /// <summary>
        /// 增加 Factorial 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddFactorialHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<ISocketChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new LoggingHandler("CONN"));
                pipeline.AddLast(new BigIntegerDecoder());
                pipeline.AddLast(new BigIntegerEncoder());
                pipeline.AddLast(channelHandler);
            });
        }

        /// <summary>
        /// 增加 QuoteOfTheMoment 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddQuoteOfTheMomentHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<IChannel>(tlsCertificate: null, pipeline =>
            {
                pipeline.AddLast("Quote", channelHandler);
            });
        }

        /// <summary>
        /// 增加 SecureChat 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddSecureChatHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<ISocketChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                pipeline.AddLast(new StringEncoder());
                pipeline.AddLast(new StringDecoder());
                pipeline.AddLast(channelHandler);
            });
        }

        /// <summary>
        /// 增加 Telnet 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddTelnetHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<ISocketChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                pipeline.AddLast(new StringEncoder());
                pipeline.AddLast(new StringDecoder());
                pipeline.AddLast(channelHandler);
            });
        }

        /// <summary>
        /// 增加 WebSocket 处理程序。
        /// </summary>
        /// <typeparam name="TChannelHandler">指定的信道处理程序类型。</typeparam>
        /// <param name="wrapper">给定的 <see cref="IBootstrapWrapper"/>。</param>
        /// <param name="tlsCertificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <param name="channelHandler">给定的 <see cref="IChannelHandler"/>。</param>
        /// <returns>返回 <see cref="IBootstrapWrapper"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "wrapper")]
        public static IBootstrapWrapper AddWebSocketHandler<TChannelHandler>(this IBootstrapWrapper wrapper,
            X509Certificate2 tlsCertificate, TChannelHandler channelHandler)
            where TChannelHandler : IChannelHandler
        {
            wrapper.NotNull(nameof(wrapper));

            return wrapper.AddChannelHandlerAsync<IChannel>(tlsCertificate, pipeline =>
            {
                pipeline.AddLast(
                    new HttpClientCodec(),
                    new HttpObjectAggregator(8192),
                    WebSocketClientCompressionHandler.Instance,
                    channelHandler);
            },
            addTlsPipelineName: true);
        }

    }
}
