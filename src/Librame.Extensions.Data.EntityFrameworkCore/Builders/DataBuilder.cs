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

namespace Librame.Extensions.Data
{
    using Core;

    class DataBuilder : AbstractExtensionBuilder, IDataBuilder
    {
        public DataBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IDataBuilder>(this);
        }

    }
}
