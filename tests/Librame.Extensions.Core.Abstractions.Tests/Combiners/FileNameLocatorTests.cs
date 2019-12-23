using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

    public class FileNameCombinerTests
    {
        [Fact]
        public void AllTest()
        {
            var path = @"c:\test\file.ext";

            var combiner = path.AsFileNameCombiner();
            Assert.Equal("file", combiner.BaseName);
            Assert.Equal(".ext", combiner.Extension);
            Assert.Equal("file.ext", combiner.ToString());

            Assert.Equal(combiner, combiner.ChangeBaseName("file1"));
            Assert.Equal(combiner, combiner.ChangeExtension(".txt"));

            Assert.NotEqual(combiner, combiner.NewBaseName("file"));
            Assert.NotEqual(combiner, combiner.NewExtension(".ext"));
        }
    }
}
