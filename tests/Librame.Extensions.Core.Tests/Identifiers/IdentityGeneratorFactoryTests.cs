using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;
    using Services;

    public class IdentityGeneratorFactoryTests
    {
        [Fact]
        public void AllTest()
        {
            var provider = TestServiceProvider.Current;

            var clock = provider.GetRequiredService<IClockService>();
            var factory = provider.GetRequiredService<IIdentityGeneratorFactory>();

            var guidGenerator = factory.GetGenerator<Guid>();
            var guid = guidGenerator.GenerateId(clock);
            Assert.NotEqual(Guid.Empty, guid);

            var longGenerator = factory.GetGenerator<long>();
            var id = longGenerator.GenerateId(clock);
            Assert.True(id > 0);
        }

    }
}
