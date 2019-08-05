using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AssemblyHelperTests
    {
        [Fact]
        public void AllTest()
        {
            AssemblyHelper.CurrentDomainAssemblies.ForEach(assembly =>
            {
                Assert.NotNull(assembly);
            });

            AssemblyHelper.CurrentDomainAssembliesWithoutSystem.ForEach(assembly =>
            {
                Assert.NotNull(assembly);
            });

            Assert.True(AssemblyHelper.CurrentDomainAssemblies.Length
                > AssemblyHelper.CurrentDomainAssembliesWithoutSystem.Length);
        }
    }
}
