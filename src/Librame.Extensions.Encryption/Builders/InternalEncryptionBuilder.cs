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

    /// <summary>
    /// 内部加密构建器。
    /// </summary>
    internal class InternalEncryptionBuilder : AbstractBuilder<EncryptionBuilderOptions>, IEncryptionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalEncryptionBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="EncryptionBuilderOptions"/>。</param>
        public InternalEncryptionBuilder(IBuilder builder, EncryptionBuilderOptions options)
            : base(builder, options)
        {
            Services.AddSingleton<IEncryptionBuilder>(this);
        }

    }
}
