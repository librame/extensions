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
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty.Internal
{
    using Encryption;
    using Extensions;

    /// <summary>
    /// 内部 Telnet 客户端通道服务。
    /// </summary>
    internal class InternalTelnetClient : AbstractChannelService<InternalTelnetClient>, ITelnetClient
    {
        private readonly ClientOptions _clientOptions;


        /// <summary>
        /// 构造一个 <see cref="InternalTelnetClient"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{ChannelOptions}"/>。</param>
        public InternalTelnetClient(ISigningCredentialsService signingCredentials,
            ILoggerFactory loggerFactory, IOptions<ChannelOptions> options)
            : base(signingCredentials, loggerFactory, options)
        {
            _clientOptions = Options.TelnetClient;
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
            if (null == handlerFactory)
                handlerFactory = () => new InternalTelnetClientHandler(this);

            host = host.HasOrDefault(_clientOptions.Host);
            port = port.HasOrDefault(_clientOptions.Port);

            var group = new MultithreadEventLoopGroup();

            X509Certificate2 cert = null;
            string targetHost = null;

            if (_clientOptions.UseSSL)
            {
                var credentials = SigningCredentials.GetSigningCredentials(_clientOptions.SigningCredentialsKey);
                cert = credentials.ResolveCertificate();
                targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
            }

            try
            {
                var bootstrap = new Bootstrap();

                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        if (cert != null)
                        {
                            pipeline.AddLast(
                                new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true),
                                new ClientTlsSettings(targetHost)));
                        }

                        pipeline.AddLast(new DelimiterBasedFrameDecoder(8192, Delimiters.LineDelimiter()));
                        pipeline.AddLast(new StringEncoder());
                        pipeline.AddLast(new StringDecoder());
                        pipeline.AddLast(handlerFactory.Invoke());
                    }));
                
                var address = IPAddress.Parse(host);
                var bootstrapChannel = await bootstrap.ConnectAsync(new IPEndPoint(address, port.Value));

                configureProcess.Invoke(bootstrapChannel);

                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                group.ShutdownGracefullyAsync().Wait(1000);
            }
        }

    }
}
