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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class TabulationNotificationHandler<TTabulation> : AbstractNotificationHandler<TabulationNotification<TTabulation>>
        where TTabulation : class
    {
        public TabulationNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(TabulationNotification<TTabulation> notification,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunOrCancelAsync(() =>
            {
                if (notification.Adds.IsNotEmpty())
                    Logger.LogInformation($"{notification.Adds.Count} tabulations added.");

                if (notification.Updates.IsNotEmpty())
                    Logger.LogInformation($"{notification.Updates.Count} tabulations updated.");

                if (notification.Removes.IsNotEmpty())
                    Logger.LogInformation($"{notification.Removes.Count} tabulations removed.");

                return Task.CompletedTask;
            });
        }

    }
}
