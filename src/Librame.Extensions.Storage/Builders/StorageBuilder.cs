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

namespace Librame.Extensions.Storage.Builders
{
    using Core.Builders;

    internal class StorageBuilder : AbstractExtensionBuilder, IStorageBuilder
    {
        public StorageBuilder(IExtensionBuilder builder, StorageBuilderDependency dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton<IStorageBuilder>(this);
        }

    }
}
