using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;

    public class SecurityIdentifierProtectorTests
    {
        [Fact]
        public void AllTest()
        {
            var protector = TestServiceProvider.Current.GetRequiredService<ISecurityIdentifierProtector>();

            var initialIndex = protector.KeyRing.CurrentIndex;
            var str = nameof(SecurityIdentifierProtectorTests);

            var protect = protector.Protect(initialIndex, str);
            Assert.NotEmpty(protect);
            Assert.NotEqual(str, protect);

            var unprotect = protector.Unprotect(initialIndex, protect);
            Assert.Equal(str, unprotect);
        }

    }
}
