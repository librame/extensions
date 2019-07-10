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
    /// <summary>
    /// 服务数据构建器静态扩展。
    /// </summary>
    public static class ServiceDataBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddServices(this IDataBuilder builder)
        {
            builder.Services.AddScoped<IAuditService, InternalAuditService>();
            builder.Services.AddScoped<IClockService, InternalClockService>();
            builder.Services.AddScoped<ITenantService, InternalTenantService>();

            builder.Services.AddScoped<IIdentifierService, IdentifierServiceBase>();
            builder.Services.AddScoped(typeof(IInitializerService<>), typeof(InitializerServiceBase<>));

            return builder;
        }

    }
}
