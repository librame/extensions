using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class AbstractionFileNameCombinerExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var files = new string[] { "1.txt", "2.txt", "3.txt", "4.txt", "5.txt" };
            var ext = ".txt";

            var combiners = files.AsFileNameCombiners();
            foreach (var combiner in combiners)
                Assert.Equal(ext, combiner.Extension);

            ext = ".ext";
            combiners.ChangeExtension(ext);
            foreach (var combiner in combiners)
                Assert.Equal(ext, combiner.Extension);
        }
    }
}
