using System;
using Xunit;

namespace Librame.Extensions.Encryption.Tests
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

            if (identifier.TryToGuid(out var g))
                Assert.NotEmpty(g.ToString());

            Assert.NotEmpty(identifier.ToShortString(DateTimeOffset.UtcNow));
        }

    }
}
