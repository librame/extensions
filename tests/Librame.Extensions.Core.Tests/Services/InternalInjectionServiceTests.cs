using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class InternalInjectionServiceTests
    {
        [Fact]
        public void AllTest()
        {
            var service = TestServiceProvider.Current.GetRequiredService<InjectionServiceTest>();
            service.InjectTest();
        }
    }


    public class InjectionServiceTest
    {
        [InjectionService]
        IBuilder _fieldBuilder = null;


        public InjectionServiceTest(IInjectionService injectionService)
        {
            injectionService.Inject(this);
        }


        [InjectionService]
        public IBuilder PropertyBuilder { get; set; }


        public void InjectTest()
        {
            _fieldBuilder.NotNull(nameof(_fieldBuilder));
            PropertyBuilder.NotNull(nameof(PropertyBuilder));
        }

    }
}
