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

namespace Librame.Extensions.Encryption.Builders
{
    using Core.Builders;

    internal class EncryptionBuilder : AbstractExtensionBuilder, IEncryptionBuilder
    {
        public EncryptionBuilder(IExtensionBuilder builder, EncryptionBuilderDependency dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton<IEncryptionBuilder>(this);
        }

    }
}
