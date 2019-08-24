#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Transport.Channels;
using System.Collections.Concurrent;
using System.Numerics;

namespace Librame.Extensions.Network.DotNetty
{
    class FactorialClientHandler : SimpleChannelInboundHandler<BigInteger>
    {
        private readonly IFactorialClient _client;
        //private readonly ILogger _logger;

        private IChannelHandlerContext _context;
        private int _receivedMessages;
        private int _next = 1;
        private readonly BlockingCollection<BigInteger> _answer = new BlockingCollection<BigInteger>();

        public BigInteger GetFactorial() => _answer.Take();


        public FactorialClientHandler(IFactorialClient client)
        {
            _client = client;
            //_logger = client.LoggerFactory.CreateLogger<InternalFactorialClientHandler>();
        }


        public override void ChannelActive(IChannelHandlerContext context)
        {
            _context = context;
            SendNumbers();
        }

        protected override void ChannelRead0(IChannelHandlerContext context, BigInteger message)
        {
            _receivedMessages++;

            if (_receivedMessages == _client.Options.FactorialClient.Count)
                context.CloseAsync().ContinueWith(t => _answer.Add(message));
        }

        private void SendNumbers()
        {
            for (int i = 0; (i < 4096) && (_next <= _client.Options.FactorialClient.Count); i++)
            {
                _context.WriteAsync(new BigInteger(_next));
                _next++;
            }

            _context.Flush();
        }

    }
}
