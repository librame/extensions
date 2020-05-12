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
    using Identifiers;

    static class IdentifierCoreBuilderExtensions
    {
        internal static ICoreBuilder AddIdentifiers(this ICoreBuilder builder)
        {
            builder.Services.TryAddScoped<ISecurityIdentifierKeyRing, SecurityIdentifierKeyRing>();
            builder.Services.TryAddScoped<ISecurityIdentifierProtector, SecurityIdentifierProtector>();

            return builder;
        }

    }
}
