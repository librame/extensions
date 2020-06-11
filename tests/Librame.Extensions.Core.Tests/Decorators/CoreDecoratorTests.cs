using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Services;

    public class CoreDecoratorTests
    {
        [Fact]
        public void AllTest()
        {
            var test = TestServiceProvider.Current.GetRequiredService<TestService>();
            var testDecorator = TestServiceProvider.Current.GetRequiredService<TestServiceDecorator>();
            Assert.NotEqual(test.Message, testDecorator.Message);

            var impl = TestServiceProvider.Current.GetRequiredService<TestServiceImplementation>();
            var implDecorator = TestServiceProvider.Current.GetRequiredService<TestServiceImplementationDecorator>();
            Assert.NotEqual(impl.Message, implDecorator.OtherMessage);

            Assert.NotEqual(test.Message, impl.Message);
            Assert.NotEqual(testDecorator.Message, implDecorator.OtherMessage);

            Assert.Equal("1", test.Message);
            Assert.Equal("2", impl.Message);
            Assert.Equal("12", testDecorator.Message);
            Assert.Equal("21", implDecorator.OtherMessage);
        }
    }
}
