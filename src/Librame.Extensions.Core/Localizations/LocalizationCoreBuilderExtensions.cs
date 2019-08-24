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
using Microsoft.Extensions.Localization;

namespace Librame.Extensions.Core
{
    static class LocalizationCoreBuilderExtensions
    {
        public static ICoreBuilder AddLocalizations(this ICoreBuilder builder)
        {
            builder.Services.AddScoped(typeof(IExpressionStringLocalizer<>), typeof(ExpressionStringLocalizer<>));
            builder.Services.TryReplace<IStringLocalizerFactory, ExpressionStringLocalizerFactory>();

            return builder;
        }

    }
}
