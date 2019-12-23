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
    using Dependencies;

    static class DependencyCoreBuilderExtensions
    {
        internal static ICoreBuilder AddDependencies(this ICoreBuilder builder)
        {
            builder.Services.TryReplace(typeof(IOptionsFactory<>), typeof(OptionsDependencyFactory<>));

            return builder;
        }

    }
}
