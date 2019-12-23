using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Utilities;

    public class AssemblyUtilityTests
    {
        [Fact]
        public void AllTest()
        {
            Assert.NotEmpty(AssemblyUtility.CurrentAssemblies);
            Assert.NotEmpty(AssemblyUtility.CurrentAssembliesWithoutSystem);
        }
    }
}
