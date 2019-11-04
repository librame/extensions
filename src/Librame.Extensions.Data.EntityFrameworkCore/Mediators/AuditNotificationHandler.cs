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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class AuditNotificationHandler<TAudit, TAuditProperty> : AbstractNotificationHandler<AuditNotification<TAudit, TAuditProperty>>
        where TAudit : class
        where TAuditProperty : class
    {
        public AuditNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(AuditNotification<TAudit, TAuditProperty> notification, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                Logger.LogInformation($"{notification.Audits.Count} Audits have been added.");

                return Task.CompletedTask;
            });
        }

    }
}
