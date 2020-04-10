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
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Core.Builders
{
    using Mediators;

    static class MediatorCoreBuilderExtensions
    {
        internal static ICoreBuilder AddMediators(this ICoreBuilder builder)
        {
            var behaviorType = typeof(IRequestPipelineBehavior<,>);
            builder.Services.TryAddEnumerable(behaviorType, ServiceLifetime.Transient,
                typeof(RequestPreProcessorBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            builder.Services.TryAddTransient(typeof(IRequestHandlerWrapper<,>), typeof(RequestHandlerWrapper<,>));
            builder.Services.TryAddTransient(typeof(INotificationHandlerWrapper<>), typeof(NotificationHandlerWrapper<>));

            builder.Services.TryAddTransient<IMediator, ServiceFactoryMediator>();

            return builder;
        }

    }
}
