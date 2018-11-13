﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Transport.Libuv;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.DotNetty
{
    using Encryption;
    using Extensions;

    /// <summary>
    /// 内部回流服务端通道服务。
    /// </summary>
    internal class InternalEchoServer : AbstractChannelService<InternalEchoServer>, IEchoServer
    {
        private readonly ServerOptions _serverOptions;


        /// <summary>
        /// 构造一个 <see cref="InternalEchoServer"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="ISigningCredentialsProvider"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{ChannelOptions}"/>。</param>
        public InternalEchoServer(ISigningCredentialsProvider provider,
            ILoggerFactory loggerFactory, IOptions<ChannelOptions> options)
            : base(provider, loggerFactory, options)
        {
            _serverOptions = Options.EchoServer;
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
            Func<IChannelHandler> handlerFactory = null, string host = null, int port = 0)
        {
            handlerFactory = handlerFactory.AsValueOrDefault(() => new InternalEchoServerHandler(this));
            host = host.AsValueOrDefault(_serverOptions.Host);
            port = port.AsValueOrDefault(_serverOptions.Port).NotOutOfPortNumberRange(nameof(port));

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
                var credentials = Provider.GetSigningCredentials(_serverOptions.SigningCredentialsKey);
                tlsCertificate = credentials.ResolveCertificate();
            }

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);

                if (_serverOptions.UseLibuv)
                    bootstrap.Channel<TcpServerChannel>();
                else
                    bootstrap.Channel<TcpServerSocketChannel>();

                bootstrap
                    .Option(ChannelOption.SoBacklog, 100)
                    .Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        var pipeline = channel.Pipeline;
                        if (tlsCertificate != null)
                            pipeline.AddLast("tls", TlsHandler.Server(tlsCertificate));

                        pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        pipeline.AddLast("echo", handlerFactory.Invoke());
                    }));

                var address = IPAddress.Parse(host);
                var bootstrapChannel = await bootstrap.BindAsync(new IPEndPoint(address, port));

                configureProcess?.Invoke(bootstrapChannel);

                await bootstrapChannel.CloseAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
            finally
            {
                Task.WaitAll(bossGroup.ShutdownGracefullyAsync(_serverOptions.QuietPeriod, _serverOptions.TimeOut),
                    workerGroup.ShutdownGracefullyAsync(_serverOptions.QuietPeriod, _serverOptions.TimeOut));
            }
        }

    }
}