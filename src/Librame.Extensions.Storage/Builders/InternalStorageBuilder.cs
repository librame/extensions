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

    /// <summary>
    /// 内部存储构建器。
    /// </summary>
    internal class InternalStorageBuilder : AbstractExtensionBuilder, IStorageBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalStorageBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        public InternalStorageBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IStorageBuilder>(this);
        }

    }
}
