using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

    public class DomainNameCombinerTests
    {
        [Fact]
        public void AllTest()
        {
            var uriString = "https://1.2.3.4.developer.microsoft.com/en-us/fabric#/get-started";

            var combiner = uriString.GetHost().AsDomainNameCombiner();

            Assert.Equal("com", combiner.Root);
            Assert.Equal("microsoft.com", combiner.TopLevel);
            Assert.Equal("developer.microsoft.com", combiner.SecondLevel);
            Assert.Equal("4.developer.microsoft.com", combiner.ThirdLevel);

            Assert.Equal(3, combiner.OtherLevelSegments.Count); // { 1, 2, 3 }

            Assert.Equal("microsoft", combiner.TopLevelSegment);
            Assert.Equal("developer", combiner.SecondLevelSegment);
            Assert.Equal("4", combiner.ThirdLevelSegment);

            combiner.ChangeDomainName("google.org");

            Assert.NotEqual("com", combiner.Root);
            Assert.NotEqual("microsoft.com", combiner.TopLevel);
            Assert.Equal("org", combiner.Root);
            Assert.Equal("google.org", combiner.TopLevel);
            Assert.Equal("developer.google.org", combiner.SecondLevel);
            Assert.Equal("4.developer.google.org", combiner.ThirdLevel);

            Assert.Equal(3, combiner.OtherLevelSegments.Count); // { 1, 2, 3 }

            Assert.Equal("google", combiner.TopLevelSegment);
            Assert.Equal("developer", combiner.SecondLevelSegment);
            Assert.Equal("4", combiner.ThirdLevelSegment);

            var onlyTwoLevels = combiner.GetOnlyTwoLevels();
            Assert.Equal("1.2.3.4.developer", onlyTwoLevels.Child);
            Assert.Equal("google.org", onlyTwoLevels.Parent);

            Assert.NotEqual(combiner, combiner.NewDomainName("microsoft.com"));

            Assert.Throws<NotSupportedException>(() => "192.168.0.1".AsDomainNameCombiner());
        }
    }
}
