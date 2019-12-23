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

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;

    internal class DataBuilder : AbstractExtensionBuilder, IDataBuilder
    {
        public DataBuilder(IExtensionBuilder builder, DataBuilderDependency dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton<IDataBuilder>(this);
        }
    }
}
