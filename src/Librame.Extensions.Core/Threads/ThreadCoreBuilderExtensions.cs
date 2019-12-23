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
    using Threads;

    static class ThreadCoreBuilderExtensions
    {
        internal static ICoreBuilder AddThreads(this ICoreBuilder builder)
        {
            builder.Services.TryAddScoped<IMemoryLocker, MemoryLocker>();

            return builder;
        }

    }
}
