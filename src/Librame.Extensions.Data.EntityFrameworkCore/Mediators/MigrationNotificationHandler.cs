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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class MigrationNotificationHandler<TMigration, TGenId> : AbstractNotificationHandler<MigrationNotification<TMigration, TGenId>>
        where TMigration : DataMigration<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        public MigrationNotificationHandler(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public override Task HandleAsync(MigrationNotification<TMigration, TGenId> notification, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                Logger.LogInformation($"{notification.Migration} have been registed.");

                return Task.CompletedTask;
            });
        }

    }
}
