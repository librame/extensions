using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class DependencyProxyTests
    {
        [Fact]
        public void AllTest()
        {
            var proxy = DispatchProxy.Create<ITestSource, TestSourceProxy>();

            proxy.Name = nameof(TestSource);
            Assert.Equal(proxy.Name, proxy.NameChanged);

            proxy.Name = nameof(TestSourceProxy);
            Assert.Equal(proxy.Name, proxy.NameChanged);

            Assert.Throws<ValidationException>(() =>
            {
                proxy.Name = null;
            });
        }

    }
}
