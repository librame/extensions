using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Singletons;
    using Utilities;

    public class AbstractSingletonTests
    {
        [Fact]
        public void AllTest()
        {
            var guid = SealedGuidSingleton.Default.Guid;

            for (var i = 0; i < 10; i++)
                Assert.Equal(guid, SealedGuidSingleton.Default.Guid);
        }

        [Fact]
        public void PerformanceTest()
        {
            var results = StopwatchUtility.Run(() =>
            {
                var guid = SealedGuidSingleton.Default.Guid;

                for (var i = 0; i < 1000; i++)
                    Assert.Equal(guid, SealedGuidSingleton.Default.Guid);
            },
            () =>
            {
                var guid = LazySingleton<GuidSingleton>.Instance.Guid;

                for (var i = 0; i < 1000; i++)
                    Assert.Equal(guid, LazySingleton<GuidSingleton>.Instance.Guid);
            });

            Assert.NotEqual(results.First(), results.Last());
        }

    }
}
