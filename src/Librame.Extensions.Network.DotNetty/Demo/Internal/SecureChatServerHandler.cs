﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Groups;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Librame.Extensions.Network.DotNetty.Demo
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SecureChatServerHandler : SimpleChannelInboundHandler<string>
    {
        private readonly ISecureChatServer _server;
        private readonly ILogger _logger;

        private static volatile IChannelGroup group;


        public SecureChatServerHandler(ISecureChatServer server)
        {
            _server = server.NotNull(nameof(server));
            _logger = server.LoggerFactory.CreateLogger<SecureChatServerHandler>();
        }


        public override void ChannelActive(IChannelHandlerContext context)
        {
            var g = group;
            if (g.IsNull())
            {
                ExtensionSettings.Preference.RunLocker(() =>
                {
                    if (group.IsNull())
                        g = group = new DefaultChannelGroup(context.Executor);
                });
            }

            var message = $"Welcome to {Dns.GetHostName()} secure chat server!\n";

            context.WriteAndFlushAsync(message);
            g.Add(context.Channel);
            _logger.LogInformation(message);
        }

        class EveryOneBut : IChannelMatcher
        {
            private readonly IChannelId _id;

            public EveryOneBut(IChannelId id)
            {
                _id = id;
            }

            public bool Matches(IChannel channel) => channel.Id != _id;
        }

        protected override void ChannelRead0(IChannelHandlerContext context, string message)
        {
            var clientMessage = $"[{context.Channel.RemoteAddress}] {message}\n";
            var serverMessage = $"[server] {message}\n";

            group.WriteAndFlushAsync(clientMessage, new EveryOneBut(context.Channel.Id));
            context.WriteAndFlushAsync(serverMessage);

            _logger.LogInformation(clientMessage);
            _logger.LogInformation(serverMessage);

            if (string.Equals(_server.Options.SecureChatServer.ExitCommand, message, StringComparison.OrdinalIgnoreCase))
            {
                context.CloseAsync();
                _logger.LogInformation(message);
            }
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
            _logger.LogInformation(exception.StackTrace);
        }

        public override bool IsSharable => true;

    }
}
