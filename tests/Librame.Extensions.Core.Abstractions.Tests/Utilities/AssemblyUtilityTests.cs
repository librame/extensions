using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AssemblyUtilityTests
    {
        [Fact]
        public void AllTest()
        {
            Assert.NotEmpty(AssemblyUtility.CurrentDomainAssemblies);
            AssemblyUtility.CurrentDomainAssemblies.ForEach(assembly =>
            {
                Assert.NotNull(assembly);
            });

            Assert.NotEmpty(AssemblyUtility.CurrentDomainAssembliesWithoutSystem);
            AssemblyUtility.CurrentDomainAssembliesWithoutSystem.ForEach(assembly =>
            {
                Assert.NotNull(assembly);
            });

            Assert.True(AssemblyUtility.CurrentDomainAssemblies.Count
                > AssemblyUtility.CurrentDomainAssembliesWithoutSystem.Count);
        }
    }
}
