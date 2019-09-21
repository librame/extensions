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

namespace Librame.Extensions.Encryption
{
    using Core;

    class EncryptionBuilder : AbstractExtensionBuilder, IEncryptionBuilder
    {
        public EncryptionBuilder(IExtensionBuilder builder, EncryptionBuilderDependencyOptions dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton<IEncryptionBuilder>(this);
        }

    }
}
