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
using System.Collections.Generic;

namespace Librame.Extensions.Core.Builders
{
    using Mediators;

    static class MediatorCoreBuilderExtensions
    {
        internal static ICoreBuilder AddMediators(this ICoreBuilder builder)
        {
            builder.Services.TryAddEnumerable(new List<ServiceDescriptor>
            {
                ServiceDescriptor.Transient(typeof(IRequestPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>)),
                ServiceDescriptor.Transient(typeof(IRequestPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>))
            });

            builder.Services.TryAddTransient(typeof(IRequestHandlerWrapper<,>), typeof(RequestHandlerWrapper<,>));
            builder.Services.TryAddTransient(typeof(INotificationHandlerWrapper<>), typeof(NotificationHandlerWrapper<>));

            builder.Services.TryAddScoped<IMediator, ServiceFactoryMediator>();

            return builder;
        }

    }
}
