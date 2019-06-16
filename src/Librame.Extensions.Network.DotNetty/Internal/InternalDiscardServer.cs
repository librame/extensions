#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Internal
{
    using Encryption;
    using Extensions;

    /// <summary>
    /// 内部弃用服务端通道服务。
    /// </summary>
    internal class InternalDiscardServer : AbstractChannelService<InternalDiscardServer>, IDiscardServer
    {
        private readonly ServerOptions _serverOptions;


        /// <summary>
        /// 构造一个 <see cref="InternalDiscardServer"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{ChannelOptions}"/>。</param>
        public InternalDiscardServer(ISigningCredentialsService signingCredentials,
            ILoggerFactory loggerFactory, IOptions<DotNettyOptions> options)
            : base(signingCredentials, loggerFactory, options)
        {
            _serverOptions = Options.DiscardServer;
        }


        /// <summary>
        /// 异步启动。
        /// </summary>
        /// <param name="configureProcess">给定的配置处理方法。</param>
        /// <param name="handlerFactory">给定的通道处理程序工厂方法（可选）。</param>
        /// <param name="host">给定要启动的主机（可选；默认使用选项配置）。</param>
        /// <param name="port">给定要启动的端口（可选；默认使用选项配置）。</param>
        /// <returns>返回一个异步操作。</returns>
        public async Task StartAsync(Action<IChannel> configureProcess,
            Func<IChannelHandler> handlerFactory = null, string host = null, int? port = null)
        {
            if (handlerFactory.IsNull())
                handlerFactory = () => new InternalDiscardServerHandler(this);

            host = host.EnsureValue(_serverOptions.Host);
            port = port.EnsureValue(_serverOptions.Port);

            var bossGroup = new MultithreadEventLoopGroup(1);
            var workerGroup = new MultithreadEventLoopGroup();

            X509Certificate2 tlsCertificate = null;

            if (_serverOptions.UseSSL)
            {
                var credentials = SigningCredentials.GetSigningCredentials(_serverOptions.SigningCredentialsKey);
                tlsCertificate = credentials.ResolveCertificate();
            }

            try
            {
                var bootstrap = new ServerBootstrap();

                bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("LSTN"))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        if (tlsCertificate.IsNotNull())
                            pipeline.AddLast(TlsHandler.Server(tlsCertificate));

                        pipeline.AddLast(new LoggingHandler("CONN"));
                        pipeline.AddLast(handlerFactory.Invoke());
                    }));

                var address = IPAddress.Parse(host);
                var bootstrapChannel = await bootstrap.BindAsync(new IPEndPoint(address, port.Value));

                configureProcess?.Invoke(bootstrapChannel);

                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
            }
        }

    }
}
