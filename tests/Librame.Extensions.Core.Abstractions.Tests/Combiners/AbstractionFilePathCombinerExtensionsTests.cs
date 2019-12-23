using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

    public class AbstractionFilePathCombinerExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var files = new string[] { "1.txt", "2.txt", "3.txt", "4.txt", "5.txt" };
            var dir = @"d:\123";

            var combiners = files.AsFilePathCombiners(dir);
            foreach (var combiner in combiners)
                Assert.Equal(dir, combiner.BasePath);

            dir = @"c:\test";
            combiners.ChangeBasePath(dir);
            foreach (var combiner in combiners)
                Assert.Equal(dir, combiner.BasePath);
        }
    }
}
