using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Core.Tests
{
    public sealed class TestLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
            => throw new NotImplementedException();

        public ILogger CreateLogger(string categoryName)
            => null;

        public void Dispose()
        {
        }
    }
}
