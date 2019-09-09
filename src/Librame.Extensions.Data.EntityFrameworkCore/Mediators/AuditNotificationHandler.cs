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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    class AuditNotificationHandler : AbstractNotificationHandler<AuditNotification>
    {
        public AuditNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(AuditNotification notification, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                Logger.LogInformation($"{notification.Audits.Count} Audits have been added.");

                return Task.CompletedTask;
            });
        }

    }
}
