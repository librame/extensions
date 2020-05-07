﻿using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

    public class FilePathCombinerTests
    {
        [Fact]
        public void AllTest()
        {
            var path = @"c:\test\file.ext";

            var combiner = path.AsFilePathCombiner();
            Assert.Equal(@"c:\test\", combiner.BasePath);
            Assert.Equal("file.ext", combiner.FileName);
            Assert.Equal(path, combiner.ToString());

            Assert.Equal(combiner, combiner.ChangeBasePath(@"d:\123"));
            Assert.Equal(combiner, combiner.ChangeFileName("newfile.ext"));

            Assert.NotEqual(combiner, combiner.WithBasePath(@"c:\test"));
            Assert.NotEqual(combiner, combiner.WithFileName("file.ext"));
        }

    }
}
