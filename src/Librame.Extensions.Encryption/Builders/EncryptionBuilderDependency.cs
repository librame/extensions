#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Builders
{
    using Core.Builders;

    /// <summary>
    /// 加密构建器依赖。
    /// </summary>
    public class EncryptionBuilderDependency : AbstractExtensionBuilderDependency<EncryptionBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="EncryptionBuilderDependency"/>。
        /// </summary>
        public EncryptionBuilderDependency()
            : base(nameof(EncryptionBuilderDependency))
        {
        }

    }
}
