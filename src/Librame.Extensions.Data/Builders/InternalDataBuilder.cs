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
    using Builders;

    /// <summary>
    /// 内部数据构建器。
    /// </summary>
    internal class InternalDataBuilder : DefaultBuilder, IDataBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalDataBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        public InternalDataBuilder(IBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IDataBuilder>(this);
        }

    }
}
