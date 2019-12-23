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
using Microsoft.Extensions.Localization;

namespace Librame.Extensions.Core.Builders
{
    using Localizers;

    static class LocalizerCoreBuilderExtensions
    {
        internal static ICoreBuilder AddLocalizers(this ICoreBuilder builder)
        {
            builder.Services.TryAddTransient(typeof(IDictionaryStringLocalizer<>), typeof(DictionaryStringLocalizer<>));
            builder.Services.TryAddSingleton<IDictionaryStringLocalizerFactory, CoreResourceDictionaryStringLocalizerFactory>();

            builder.Services.TryReplace<IStringLocalizerFactory, CoreResourceManagerStringLocalizerFactory>();

            return builder;
        }

    }
}
