using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class CoreDecoratorTests
    {
        public class TestService : AbstractService
        {
            public TestService()
                : base()
            {
            }

            public string Message { get; set; }
                = "1";
        }

        public class TestServiceDecorator : TestService
        {
            public TestServiceDecorator(IDecorator<TestService> decorator)
                : base()
            {
                Message = decorator.Source.Message + "2";
            }
        }


        public class TestImplementation : TestService
        {
            public TestImplementation()
                : base()
            {
                Message = "2";
            }
        }

        public class TestImplementationDecorator : TestImplementation
        {
            public TestImplementationDecorator(IDecorator<TestService, TestImplementation> decorator)
                : base()
            {
                Message = decorator.Source.Message + "1";
            }
        }


        [Fact]
        public void AllTest()
        {
            var test = TestServiceProvider.Current.GetRequiredService<TestService>();
            var testDecorator = TestServiceProvider.Current.GetRequiredService<TestServiceDecorator>();
            Assert.NotEqual(test.Message, testDecorator.Message);

            var impl = TestServiceProvider.Current.GetRequiredService<TestImplementation>();
            var implDecorator = TestServiceProvider.Current.GetRequiredService<TestImplementationDecorator>();
            Assert.NotEqual(impl.Message, implDecorator.Message);

            Assert.NotEqual(test.Message, impl.Message);
            Assert.NotEqual(testDecorator.Message, implDecorator.Message);

            Assert.Equal("1", test.Message);
            Assert.Equal("2", impl.Message);
            Assert.Equal("12", testDecorator.Message);
            Assert.Equal("21", implDecorator.Message);
        }
    }
}
