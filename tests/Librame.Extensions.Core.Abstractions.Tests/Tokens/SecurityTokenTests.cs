using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Tokens;

    public class SecurityTokenTests
    {
        [Fact]
        public void AllTest()
        {
            var token = SecurityToken.New();

            var str = token.ToString();
            Assert.NotEmpty(str);

            Assert.True(SecurityToken.TryGetToken(str, out var result));
            Assert.Equal(result, token);

            Assert.NotEmpty(token.ToGuid().ToString());
            Assert.NotEmpty(token.ToShortString(DateTimeOffset.UtcNow));
        }

    }
}
