#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class EnvironmentService : AbstractService, IEnvironmentService
    {
        public EnvironmentService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public Task<IEnvironmentInfo> GetEnvironmentInfoAsync(CancellationToken cancellationToken = default)
        {
            IEnvironmentInfo info = new EnvironmentInfo();
            Logger.LogInformation($"Refresh environment info at {DateTimeOffset.Now.ToString(CultureInfo.CurrentCulture)}");

            return Task.FromResult(info);
        }

    }
}
