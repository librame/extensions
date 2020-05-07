using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Utilities;

    public class IPAddressUtilityTests
    {
        [Fact]
        public async void AllTest()
        {
            (var v4, var v6) = await IPAddressUtility
                .GetLocalIPv4AndIPv6AddressAsync()
                .ConfigureAndResultAsync();

            Assert.True(v4.IsIPv4());
            Assert.True(v6.IsIPv6());

            Assert.False(v4.IsIPv4Loopback());
            Assert.False(v6.IsIPv6Loopback());

            Assert.False(v4.IsLoopback());
            Assert.False(v6.IsLoopback());
        }

    }
}
