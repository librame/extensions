using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ProbabilityListTests
    {
        [Fact]
        public void AllTest()
        {
            var sample = new Dictionary<string, double>();
            sample.Add("a", 10d);
            sample.Add("b", 5d);
            sample.Add("c", 20d);
            sample.Add("d", 15d);
            sample.Add("e", 30d);
            sample.Add("f", 40d);
            sample.Add("g", 25d);

            var probability = new ProbabilityList(sample.Values);
            Assert.Equal(sample.Count, probability.WeightRanges.Count);

            var result = new Dictionary<string, int>();
            for (var i = 0; i < 100; i++)
            {
                var randomIndex = probability.GetRandomIndex();
                var sampleKey = sample.Keys.ElementAt(randomIndex);

                if (result.ContainsKey(sampleKey))
                    result[sampleKey] += 1;
                else
                    result.Add(sampleKey, 1);
            }

            var orderby = result.OrderByDescending(k => k.Value);
            var max = orderby.First();
            var min = orderby.Last();
            Assert.True(max.Value > min.Value);
        }

    }
}
