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

    /// <summary>
    /// 内部数据构建器。
    /// </summary>
    internal class InternalDataBuilder : AbstractBuilder<DataBuilderOptions>, IDataBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalDataBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        public InternalDataBuilder(IBuilder builder, DataBuilderOptions options)
            : base(builder, options)
        {
            Services.AddSingleton<IDataBuilder>(this);
        }
    }
}
