using Xunit;

namespace Librame.Extensions.Tests
{
    public class MathExtensionsTests
    {

        [Fact]
        public void ComputeGCDTest()
        {
            Assert.Equal(12, 24.ComputeGCD(12));
        }


        [Fact]
        public void ComputeLCMTest()
        {
            var lcm = 24.ComputeLCM(12);
            Assert.Equal(24, 24.ComputeLCM(12));
        }

    }
}
