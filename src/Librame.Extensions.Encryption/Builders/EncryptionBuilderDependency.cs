#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        public EncryptionBuilderDependency(IExtensionBuilderDependency parentDependency = null)
            : this(nameof(EncryptionBuilderDependency), parentDependency)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="EncryptionBuilderDependency"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        protected EncryptionBuilderDependency(string name, IExtensionBuilderDependency parentDependency = null)
            : base(name, parentDependency)
        {
        }

    }
}
