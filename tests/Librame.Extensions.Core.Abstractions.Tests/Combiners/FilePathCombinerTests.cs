using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

    public class FilePathCombinerTests
    {
        [Fact]
        public void AllTest()
        {
            var path = @"c:\filePath.ext";

            var combiner = path.AsFilePathCombiner();
            Assert.Equal(@"c:\", combiner.BasePath);
            Assert.Equal("filePath.ext", combiner.FileName);
            Assert.Equal(path, combiner.ToString());

            Assert.Equal(combiner, combiner.ChangeBasePath(@"d:\123"));
            Assert.Equal(combiner, combiner.ChangeFileName("newFilePath.ext"));

            Assert.NotEqual(combiner, combiner.WithBasePath(@"c:\"));
            Assert.NotEqual(combiner, combiner.WithFileName("filePath.ext"));

            combiner.ChangeBasePath(@"c:\");

            var list = new List<Guid>();
            for (var i = 0; i < 10; i++)
                list.Add(Guid.NewGuid());

            // Write and Read Json
            combiner.WriteJson(list);

            var cache = combiner.ReadJson<List<Guid>>();
            Assert.Equal(list.Count, cache.Count);

            // Write and Read Secure Json
            combiner.WriteSecureJson(list);

            cache = combiner.ReadSecureJson<List<Guid>>();
            Assert.Equal(list.Count, cache.Count);

            combiner.Delete();
        }

    }
}
