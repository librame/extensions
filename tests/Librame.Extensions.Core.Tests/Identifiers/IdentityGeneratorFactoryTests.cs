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
            var factory = provider.GetRequiredService<IIdentificationGeneratorFactory>();

            var guidGenerator = factory.GetIdGenerator<Guid>();
            var guid = guidGenerator.GenerateId(clock);
            Assert.NotEqual(Guid.Empty, guid);

            var longGenerator = factory.GetIdGenerator<long>();
            var lid = longGenerator.GenerateId(clock);
            Assert.True(lid > 0);

            var stringGenerator = factory.GetIdGenerator<string>() as MonggoIdentificationGenerator;
            var strid = stringGenerator.GenerateId(clock, out var descriptor);
            Assert.NotEmpty(strid);

            var parse = MonggoIdentificationDescriptor.Parse(strid);
            Assert.Equal(parse, descriptor);
        }

    }
}
