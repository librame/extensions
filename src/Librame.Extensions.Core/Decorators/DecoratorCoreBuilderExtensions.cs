#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Core.Builders
{
    using Decorators;

    static class DecoratorCoreBuilderExtensions
    {
        internal static ICoreBuilder AddDecorators(this ICoreBuilder builder)
        {
            builder.Services.TryAddScoped(typeof(IDecorator<,>), typeof(CoreDecorator<,>));
            builder.Services.TryAddScoped(typeof(IDecorator<>), typeof(CoreDecorator<>));

            return builder;
        }

    }
}
