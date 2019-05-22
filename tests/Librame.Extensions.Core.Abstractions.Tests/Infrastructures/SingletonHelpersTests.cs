using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class SingletonHelpersTests
    {
        [Fact]
        public void AllTest()
        {
            var init = SingletonHelper.GetInstance<TestSingleton>();
            var now = SingletonHelper.GetInstance<TestSingleton>();

            Assert.Equal(init.Time.Ticks, now.Time.Ticks);
        }
    }


    public class TestSingleton
    {
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
