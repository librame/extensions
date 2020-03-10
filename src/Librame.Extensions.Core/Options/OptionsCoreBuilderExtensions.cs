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
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Core.Builders
{
    using Options;

    static class OptionsCoreBuilderExtensions
    {
        internal static ICoreBuilder AddOptions(this ICoreBuilder builder)
        {
            builder.Services.TryReplace(typeof(IOptionsFactory<>), typeof(ConsistencyOptionsFactory<>));

            return builder;
        }

    }
}
