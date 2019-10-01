using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ExpressionLocalizerTests
    {
        [Fact]
        public void AllTest()
        {
            var localizer = TestServiceProvider.Current.GetRequiredService<IExpressionLocalizer<TestResource>>();
            Assert.True(localizer[r => r.Name].ResourceNotFound);
        }

    }

    public class TestResource : IResource
    {
        public string Name { get; set; }
    }
}
