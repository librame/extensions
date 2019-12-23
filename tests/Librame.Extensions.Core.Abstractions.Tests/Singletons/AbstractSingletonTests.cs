using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Singletons;
    using Utilities;

    public class AbstractSingletonTests
    {
        public sealed class GuidSingleton : AbstractSingleton<GuidSingleton>
        {
            private GuidSingleton()
                : base()
            {
                Guid = Guid.NewGuid();
            }


            public Guid Guid { get; }
        }

        public class GuidSingletonHelper
        {
            public GuidSingletonHelper()
            {
                Guid = Guid.NewGuid();
            }


            public Guid Guid { get; }
        }



        [Fact]
        public void AllTest()
        {
            var guid = GuidSingleton.Instance.Guid;

            for (var i = 0; i < 10; i++)
                Assert.Equal(guid, GuidSingleton.Instance.Guid);
        }

        [Fact]
        public void PerformanceTest()
        {
            var results = StopwatchUtility.Run(() =>
            {
                var guid = GuidSingleton.Instance.Guid;

                for (var i = 0; i < 1000; i++)
                    Assert.Equal(guid, GuidSingleton.Instance.Guid);
            },
            () =>
            {
                var guid = LazySingleton<GuidSingletonHelper>.Instance.Guid;

                for (var i = 0; i < 1000; i++)
                    Assert.Equal(guid, LazySingleton<GuidSingletonHelper>.Instance.Guid);
            });

            Assert.NotEqual(results.First(), results.Last());
        }

    }
}
