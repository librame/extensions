using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AssemblyHelperTests
    {
        [Fact]
        public void AllTest()
        {
            Assert.NotEmpty(AssemblyHelper.CurrentDomainAssemblies);
            AssemblyHelper.CurrentDomainAssemblies.ForEach(assembly =>
            {
                Assert.NotNull(assembly);
            });

            Assert.NotEmpty(AssemblyHelper.CurrentDomainAssembliesWithoutSystem);
            AssemblyHelper.CurrentDomainAssembliesWithoutSystem.ForEach(assembly =>
            {
                Assert.NotNull(assembly);
            });

            Assert.True(AssemblyHelper.CurrentDomainAssemblies.Length
                > AssemblyHelper.CurrentDomainAssembliesWithoutSystem.Length);
        }
    }
}
