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
    static class ThreadCoreBuilderExtensions
    {
        public static ICoreBuilder AddThreads(this ICoreBuilder builder)
        {
            builder.Services.AddScoped<IMemoryLocker, MemoryLocker>();

            return builder;
        }

    }
}
