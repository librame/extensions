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

namespace Librame.Extensions.Storage
{
    using Core;

    class StorageBuilder : AbstractExtensionBuilder, IStorageBuilder
    {
        public StorageBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IStorageBuilder>(this);
        }

    }
}
