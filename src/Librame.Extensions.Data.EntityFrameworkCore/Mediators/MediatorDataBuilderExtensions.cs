#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 中介者数据构建器静态扩展。
    /// </summary>
    public static class MediatorDataBuilderExtensions
    {
        /// <summary>
        /// 添加中介者集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddMediators(this IDataBuilder builder)
        {
            builder.Services.AddSingleton<INotificationHandler<AuditNotification>, InternalAuditNotificationHandler>();

            return builder;
        }

    }
}
