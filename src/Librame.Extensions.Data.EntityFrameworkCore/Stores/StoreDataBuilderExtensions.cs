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
    static class StoreDataBuilderExtensions
    {
        public static IDataBuilder AddStores(this IDataBuilder builder)
        {
            builder.Services.AddScoped(typeof(IStoreHub<>), typeof(StoreHubBase<>));
            builder.Services.AddScoped(typeof(IStoreHub<,,>), typeof(StoreHubBase<,,>));

            return builder;
        }

    }
}
