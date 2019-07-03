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

    /// <summary>
    /// 内部审计通知处理程序。
    /// </summary>
    internal class InternalAuditNotificationHandler : AbstractNotificationHandler<AuditNotification>
    {
        private readonly ILogger<InternalAuditNotificationHandler> _logger;


        /// <summary>
        /// 构建一个 <see cref="InternalAuditNotificationHandler"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{AuditNotificationHandler}"/>。</param>
        public InternalAuditNotificationHandler(ILogger<InternalAuditNotificationHandler> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// 异步处理。
        /// </summary>
        /// <param name="notification">给定的 <see cref="AuditNotification"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public override Task HandleAsync(AuditNotification notification, CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                _logger.LogInformation($"{notification.Audits.Count} Audits have been processed.");

                return Task.CompletedTask;
            });
        }
    }
}
