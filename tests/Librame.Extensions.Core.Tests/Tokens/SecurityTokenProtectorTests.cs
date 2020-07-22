using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Tokens;

    public class SecurityTokenProtectorTests
    {
        [Fact]
        public void AllTest()
        {
            var protector = TestServiceProvider.Current.GetRequiredService<ISecurityTokenProtector>();

            var initialIndex = protector.KeyRing.CurrentIndex;
            var str = nameof(SecurityTokenProtectorTests);

            var protect = protector.Protect(initialIndex, str);
            Assert.NotEmpty(protect);
            Assert.NotEqual(str, protect);

            var unprotect = protector.Unprotect(initialIndex, protect);
            Assert.Equal(str, unprotect);
        }

    }
}
