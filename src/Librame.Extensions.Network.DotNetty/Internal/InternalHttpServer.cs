#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs.Http;
using DotNetty.Common;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Internal
{
    using Encryption;
    using Extensions;

    /// <summary>
    /// 内部 HTTP 服务端通道服务。
    /// </summary>
    internal class InternalHttpServer : AbstractChannelService<InternalHttpServer>, IHttpServer
    {
        private readonly HttpServerOptions _serverOptions;


        /// <summary>
        /// 构造一个 <see cref="InternalHttpServer"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{ChannelOptions}"/>。</param>
        public InternalHttpServer(ISigningCredentialsService signingCredentials,
            ILoggerFactory loggerFactory, IOptions<DotNettyOptions> options)
            : base(signingCredentials, loggerFactory, options)
        {
            _serverOptions = Options.HttpServer;
            ResourceLeakDetector.Level = _serverOptions.LeakDetector;
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
                handlerFactory = () => new InternalHttpServerHandler(this);

            host = host.EnsureValue(_serverOptions.Host);
            port = port.EnsureValue(_serverOptions.Port);

            Logger.LogInformation(
                   $"\n{RuntimeInformation.OSArchitecture} {RuntimeInformation.OSDescription}"
                   + $"\n{RuntimeInformation.ProcessArchitecture} {RuntimeInformation.FrameworkDescription}"
                   + $"\nProcessor Count : {Environment.ProcessorCount}\n");
            
            Logger.LogInformation($"Transport type: {(_serverOptions.UseLibuv ? "Libuv" : "Socket")}\n");

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            Logger.LogInformation($"Server garbage collection: {GCSettings.IsServerGC}");
            Logger.LogInformation($"Current latency mode for garbage collection: {GCSettings.LatencyMode}");
            Logger.LogInformation("\n");

            IEventLoopGroup bossGroup;
            IEventLoopGroup workerGroup;

            if (_serverOptions.UseLibuv)
            {
                var dispatcher = new DispatcherEventLoopGroup();
                bossGroup = dispatcher;
                workerGroup = new WorkerEventLoopGroup(dispatcher);
            }
            else
            {
                bossGroup = new MultithreadEventLoopGroup(1);
                workerGroup = new MultithreadEventLoopGroup();
            }

            X509Certificate2 tlsCertificate = null;

            if (_serverOptions.UseSSL)
            {
                var credentials = SigningCredentials.GetSigningCredentials(_serverOptions.SigningCredentialsKey);
                tlsCertificate = credentials.ResolveCertificate();
            }

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);

                if (_serverOptions.UseLibuv)
                {
                    bootstrap.Channel<TcpServerChannel>();

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                        || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        bootstrap
                            .Option(ChannelOption.SoReuseport, true)
                            .ChildOption(ChannelOption.SoReuseaddr, true);
                    }
                }
                else
                {
                    bootstrap.Channel<TcpServerSocketChannel>();
                }

                bootstrap
                    .Option(ChannelOption.SoBacklog, 8192)
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        if (tlsCertificate.IsNotNull())
                            pipeline.AddLast(TlsHandler.Server(tlsCertificate));

                        pipeline.AddLast("encoder", new HttpResponseEncoder());
                        pipeline.AddLast("decoder", new HttpRequestDecoder(4096, 8192, 8192, false));
                        pipeline.AddLast("handler", handlerFactory.Invoke());
                    }));

                var address = IPAddress.Parse(host);
                var bootstrapChannel = await bootstrap.BindAsync(new IPEndPoint(address, port.Value));
                Logger.LogInformation($"Httpd started. Listening on {bootstrapChannel.LocalAddress}");

                configureProcess.Invoke(bootstrapChannel);

                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                bossGroup.ShutdownGracefullyAsync().Wait();
            }
        }

    }
}
