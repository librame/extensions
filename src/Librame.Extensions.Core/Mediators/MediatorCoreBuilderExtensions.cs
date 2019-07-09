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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 中介者核心构建器静态扩展。
    /// </summary>
    public static class MediatorCoreBuilderExtensions
    {
        /// <summary>
        /// 添加中介者集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddMediators(this ICoreBuilder builder)
        {
            builder.Services.AddTransient(typeof(IRequestPipelineBehavior<,>), typeof(InternalRequestPreProcessorBehavior<,>));
            builder.Services.AddTransient(typeof(IRequestPipelineBehavior<,>), typeof(InternalRequestPostProcessorBehavior<,>));

            builder.Services.AddTransient(typeof(IRequestHandlerWrapper<,>), typeof(InternalRequestHandlerWrapper<,>));
            builder.Services.AddTransient(typeof(INotificationHandlerWrapper<>), typeof(InternalNotificationHandlerWrapper<>));

            builder.Services.AddTransient<IMediator, InternalMediator>();

            return builder;
        }

    }
}
