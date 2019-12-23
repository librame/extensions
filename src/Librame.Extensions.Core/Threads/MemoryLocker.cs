#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Threads
{
    using Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class MemoryLocker : AbstractDisposable, IMemoryLocker
    {
        private SemaphoreSlim _locker;


        public MemoryLocker(IOptions<CoreBuilderOptions> options)
        {
            ThreadsCount = options.NotNull(nameof(options)).Value.ThreadsCount;

            _locker = new SemaphoreSlim(ThreadsCount);
        }


        public int ThreadsCount { get; }


        public int Release(int? releaseCount = null)
            => releaseCount.HasValue ? _locker.Release(releaseCount.Value) : _locker.Release();


        public void Wait(int? millisecondsTimeout = null, TimeSpan? timeout = null,
            CancellationToken? cancellationToken = null)
        {
            if (!millisecondsTimeout.HasValue && !timeout.HasValue && !cancellationToken.HasValue)
            {
                _locker.Wait();
                return;
            }

            if (millisecondsTimeout.HasValue)
            {
                if (cancellationToken.HasValue)
                    _locker.Wait(millisecondsTimeout.Value, cancellationToken.Value);
                else
                    _locker.Wait(millisecondsTimeout.Value);
                return;
            }

            if (timeout.HasValue)
            {
                if (cancellationToken.HasValue)
                    _locker.Wait(timeout.Value, cancellationToken.Value);
                else
                    _locker.Wait(timeout.Value);
                return;
            }

            _locker.Wait(cancellationToken.Value);
        }

        public Task WaitAsync(int? millisecondsTimeout = null, TimeSpan? timeout = null,
            CancellationToken? cancellationToken = null)
        {
            if (!millisecondsTimeout.HasValue && !timeout.HasValue && !cancellationToken.HasValue)
                return _locker.WaitAsync();

            if (millisecondsTimeout.HasValue)
            {
                if (cancellationToken.HasValue)
                    return _locker.WaitAsync(millisecondsTimeout.Value, cancellationToken.Value);
                else
                    return _locker.WaitAsync(millisecondsTimeout.Value);
            }

            if (timeout.HasValue)
            {
                if (cancellationToken.HasValue)
                    return _locker.WaitAsync(timeout.Value, cancellationToken.Value);
                else
                    return _locker.WaitAsync(timeout.Value);
            }

            return _locker.WaitAsync(cancellationToken.Value);
        }


        protected override void DisposeCore()
            => _locker.Dispose();
    }
}
