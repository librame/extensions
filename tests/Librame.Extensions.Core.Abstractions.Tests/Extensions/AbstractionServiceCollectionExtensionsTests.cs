using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractionServiceCollectionExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ITestAnimal, TestCat>();
            services.AddSingleton<ITestAnimal, TestDog>();

            Assert.True(services.TryGetAll<ITestAnimal, TestDog>(out var descriptors));
            Assert.Equal(typeof(TestDog), descriptors[0].ImplementationType);

            Assert.True(services.TryReplaceAll<ITestAnimal, TestCat, TestTiger>());
            Assert.True(services.TryGetAll<ITestAnimal, TestTiger>(out descriptors));
            Assert.Equal(typeof(TestTiger), descriptors[0].ImplementationType);

            Assert.False(services.TryGetAll<ITestAnimal, TestCat>(out _));

            Assert.True(services.TryReplaceSingle<ITestAnimal>(descriptor => descriptor.ImplementationType == typeof(TestTiger),
                oldDescriptor => new ServiceDescriptor(typeof(ITestAnimal), typeof(TestCat), oldDescriptor.Lifetime)));
            Assert.True(services.TryGetAll<ITestAnimal, TestCat>(out descriptors));
            Assert.Equal(typeof(TestCat), descriptors[0].ImplementationType);
        }
    }

}
