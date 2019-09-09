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

namespace Librame.Extensions.Core
{
    static class ConverterCoreBuilderExtensions
    {
        public static ICoreBuilder AddConverters(this ICoreBuilder builder)
        {
            builder.Services.TryAddSingleton<IEncodingConverter, EncodingConverter>();
            return builder;
        }

    }
}
