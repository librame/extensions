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

namespace Librame.Extensions.Core
{
    static class LocalizerCoreBuilderExtensions
    {
        public static ICoreBuilder AddLocalizers(this ICoreBuilder builder)
        {
            builder.Services.TryAddTransient(typeof(IExpressionLocalizer<>), typeof(ExpressionLocalizer<>));
            builder.Services.AddTransient(typeof(IDictionaryExpressionLocalizer<>), typeof(DictionaryExpressionLocalizer<>));
            
            builder.Services.TryReplace<IStringLocalizerFactory, CoreResourceManagerStringLocalizerFactory>();
            builder.Services.TryAddSingleton<IDictionaryStringLocalizerFactory, CoreResourceDictionaryStringLocalizerFactory>();
            
            return builder;
        }

    }
}
