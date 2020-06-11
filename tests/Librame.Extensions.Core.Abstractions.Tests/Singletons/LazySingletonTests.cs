using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Singletons;

    public class LazySingletonTests
    {
        [Fact]
        public void AllTest()
        {
            var now = LazySingleton.GetInstance<DateTimeSingleton>().Time;

            for (var i = 0; i < 10; i++)
                Assert.Equal(now.Ticks, LazySingleton.GetInstance<DateTimeSingleton>().Time.Ticks);
        }

    }
}
