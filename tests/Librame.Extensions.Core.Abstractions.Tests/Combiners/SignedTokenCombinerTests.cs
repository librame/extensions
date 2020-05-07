using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Combiners;

    public class SignedTokenCombinerTests
    {
        [Fact]
        public void AllTest()
        {
            var dataSegments = "1,2,3,4,5,6".Split(',');
            var otherSegments = "7,8,9,0".Split(',');

            var combiner = new SignedTokenCombiner(dataSegments);

            combiner.AddDataSegments(otherSegments);
            Assert.Equal(10, combiner.Count);

            combiner.RemoveDataSegments("7", otherSegments.Length);
            Assert.Equal(6, combiner.Count);

            var addCombiner = combiner.WithAddDataSegments(otherSegments);
            Assert.NotEqual(combiner.Count, addCombiner.Count);

            addCombiner.RemoveDataSegments("7", otherSegments.Length);
            Assert.Equal(combiner.Count, addCombiner.Count);
        }

    }
}
