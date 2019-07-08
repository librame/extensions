using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InjectionServiceTest
    {
        [InjectionService]
        ICoreBuilder _fieldBuilder = null;


        public InjectionServiceTest(IInjectionService injectionService)
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


    public class InternalInjectionServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<InjectionServiceTest>();
            service.InjectTest();
        }
    }
}
