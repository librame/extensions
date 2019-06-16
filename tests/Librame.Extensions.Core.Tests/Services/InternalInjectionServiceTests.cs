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
}
