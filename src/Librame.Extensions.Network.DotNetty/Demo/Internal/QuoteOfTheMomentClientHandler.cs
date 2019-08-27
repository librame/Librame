﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Network.DotNetty
{
    class QuoteOfTheMomentClientHandler : SimpleChannelInboundHandler<DatagramPacket>
    {
        private readonly IQuoteOfTheMomentClient _client;
        private readonly ILogger _logger;


        public QuoteOfTheMomentClientHandler(IQuoteOfTheMomentClient client)
            : base()
        {
            _client = client;
            _logger = client.LoggerFactory.CreateLogger<DiscardClientHandler>();
        }


        protected override void ChannelRead0(IChannelHandlerContext ctx, DatagramPacket packet)
        {
            _logger.LogInformation($"Client Received => {packet}");

            if (!packet.Content.IsReadable())
            {
                return;
            }

            var message = packet.Content.ToString(_client.Options.Encoding);
            if (!message.StartsWith("QOTM: "))
            {
                return;
            }

            _logger.LogInformation($"Quote of the Moment: {message.Substring(6)}");
            ctx.CloseAsync();
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            _logger.LogError(exception, exception.AsInnerMessage());
            context.CloseAsync();
        }

    }
}