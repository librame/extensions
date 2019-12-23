using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Tests
{
    using Threads;

    public class MemoryLockerTests
    {
        public class TestMemoryLocker : AbstractDisposable, IMemoryLocker
        {
            private readonly SemaphoreSlim _locker
                = new SemaphoreSlim(Environment.ProcessorCount);


            public int ThreadsCount
                => Environment.ProcessorCount;


            public int Release(int? releaseCount = null)
                => _locker.Release();


            public void Wait(int? millisecondsTimeout = null, TimeSpan? timeout = null, CancellationToken? cancellationToken = null)
                => _locker.Wait();

            public Task WaitAsync(int? millisecondsTimeout = null, TimeSpan? timeout = null, CancellationToken? cancellationToken = null)
                => _locker.WaitAsync();

            protected override void DisposeCore()
                => _locker.Dispose();
        }
    }
}
