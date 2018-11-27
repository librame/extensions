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
    using Builders;

    /// <summary>
    /// 内部加密构建器。
    /// </summary>
    internal class InternalEncryptionBuilder : Builder, IEncryptionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalEncryptionBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        public InternalEncryptionBuilder(IBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<IEncryptionBuilder>(this);
        }

    }
}
