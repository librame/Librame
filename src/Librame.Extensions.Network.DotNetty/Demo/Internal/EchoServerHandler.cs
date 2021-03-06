﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class EchoServerHandler : ChannelHandlerAdapter
    {
        private readonly IEchoServer _server;
        private readonly ILogger _logger;


        public EchoServerHandler(IEchoServer server)
        {
            _server = server.NotNull(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<EchoServerHandler>();
        }


        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            if (buffer.IsNotNull())
                _logger.LogInformation($"Received from client: {buffer.ToString(_server.Encoding)}");

            context.WriteAsync(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)
            => context.Flush();


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }

    }
}
