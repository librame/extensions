using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class LazySingletonTests
    {
        public class TestSingleton
        {
            public DateTime Time { get; set; } = DateTime.Now;
        }


        [Fact]
        public void AllTest()
        {
            var now = LazySingleton.GetInstance<TestSingleton>().Time;

            for (var i = 0; i < 10; i++)
                Assert.Equal(now.Ticks, LazySingleton.GetInstance<TestSingleton>().Time.Ticks);
        }

    }
}
