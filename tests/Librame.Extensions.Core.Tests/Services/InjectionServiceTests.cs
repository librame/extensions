using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Services;

    public class TestInjectionService
    {
        [InjectionService]
        ICoreBuilder _fieldBuilder = null;


        public TestInjectionService(IInjectionService injectionService)
        {
            injectionService.Inject(this);
        }


        [InjectionService]
        public ICoreBuilder PropertyBuilder { get; set; }


        public void InjectTest()
        {
            _fieldBuilder.NotNull(nameof(_fieldBuilder));
            PropertyBuilder.NotNull(nameof(PropertyBuilder));
        }

    }


    public class InjectionServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<TestInjectionService>();
            service.InjectTest();
        }
    }
}
