using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Threads;

    public class MemoryLockerTests
    {
        [Fact]
        public void AllTest()
        {
            var locker = TestServiceProvider.Current.GetRequiredService<IMemoryLocker>();

            // https://www.jianshu.com/p/a9d16eecee98
            var eatings = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                eatings.Add(Task.Run(() => Eat(locker)));
            }
            Task.WaitAll(eatings.ToArray());
        }

        static void Eat(IMemoryLocker locker)
        {
            locker.Wait();

            try
            {
                Assert.True(Task.CurrentId.Value > 0);
                Thread.Sleep(1000);
            }
            finally
            {
                Assert.True(Task.CurrentId.Value > 0);
                locker.Release();
            }
        }
    }
}
