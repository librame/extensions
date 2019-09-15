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
    static class MediatorCoreBuilderExtensions
    {
        public static ICoreBuilder AddMediators(this ICoreBuilder builder)
        {
            builder.Services.AddTransient(typeof(IRequestPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            builder.Services.AddTransient(typeof(IRequestPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

            builder.Services.AddTransient(typeof(IRequestHandlerWrapper<,>), typeof(RequestHandlerWrapper<,>));
            builder.Services.AddTransient(typeof(INotificationHandlerWrapper<>), typeof(NotificationHandlerWrapper<>));

            builder.Services.AddScoped<IMediator, ServiceFactoryMediator>();

            return builder;
        }

    }
}
