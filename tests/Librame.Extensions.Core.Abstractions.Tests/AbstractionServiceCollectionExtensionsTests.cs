using Microsoft.Extensions.DependencyInjection;
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

            Assert.True(services.TryGet<ITestAnimal, TestDog>(out ServiceDescriptor serviceDescriptor));
            Assert.Equal(typeof(TestDog), serviceDescriptor.ImplementationType);

            Assert.True(services.TryReplace<ITestAnimal, TestCat, TestTiger>());
            Assert.True(services.TryGet<ITestAnimal, TestTiger>(out serviceDescriptor));
            Assert.Equal(typeof(TestTiger), serviceDescriptor.ImplementationType);
            Assert.False(services.TryGet<ITestAnimal, TestCat>(out serviceDescriptor));
            Assert.Null(serviceDescriptor);
        }
    }


    public interface ITestAnimal
    {
    }

    public class TestCat : ITestAnimal
    {
    }

    public class TestDog : ITestAnimal
    {
    }

    public class TestTiger : ITestAnimal
    {
    }
}
