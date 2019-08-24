using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class SingletonHelperTests
    {
        public class TestSingleton
        {
            public DateTime Time { get; set; } = DateTime.Now;
        }


        [Fact]
        public void AllTest()
        {
            var now = SingletonHelper.GetInstance<TestSingleton>().Time;

            for (var i = 0; i < 10; i++)
                Assert.Equal(now.Ticks, SingletonHelper.GetInstance<TestSingleton>().Time.Ticks);
        }

    }
}
