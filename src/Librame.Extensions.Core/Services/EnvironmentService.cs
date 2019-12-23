#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Services
{
    using Threads;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class EnvironmentService : AbstractConcurrentService, IEnvironmentService
    {
        public EnvironmentService(IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(locker, loggerFactory)
        {
        }


        public Task<IEnvironmentInfo> GetEnvironmentInfoAsync(CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactory(() =>
            {
                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    IEnvironmentInfo info = new EnvironmentInfo();
                    Logger.LogInformation($"Refresh environment info at {DateTimeOffset.Now.ToString(CultureInfo.CurrentCulture)}");

                    return info;
                });
            });
        }

    }
}
