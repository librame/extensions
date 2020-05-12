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

namespace Librame.Extensions.Data.Builders
{
    using Protectors;

    static class ProtectorDataBuilderExtensions
    {
        internal static IDataBuilder AddProtectors(this IDataBuilder builder)
        {
            builder.Services.TryAddSingleton<IPrivacyDataProtector, PrivacyDataProtector>();

            return builder;
        }

    }
}
