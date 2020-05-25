#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Core.Loggers
{
    internal class NoneLoggerFactory : ILoggerFactory
    {
        public static readonly ILoggerFactory Default
            = new NoneLoggerFactory();


        private NoneLoggerFactory()
        {
        }


        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
            => null;

        public void Dispose()
        {
        }

    }
}
