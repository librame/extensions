using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SecurityIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            var identifier = SecurityIdentifier.New();

            var str = identifier.ToString();
            Assert.NotEmpty(str);

            Assert.True(SecurityIdentifier.TryGetIdentifier(str, out var result));
            Assert.Equal(result, identifier);

            Assert.NotEmpty(identifier.ToGuid().ToString());
            Assert.NotEmpty(identifier.ToShortString(DateTimeOffset.UtcNow));
        }

    }
}
