#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption
{
    using Builders;

    /// <summary>
    /// 加密构建器选项接口。
    /// </summary>
    public interface IEncryptionBuilderOptions : IBuilderOptions
    {
        /// <summary>
        /// 标识符（默认新建标识符；参考 <see cref="AlgorithmIdentifier"/>）。
        /// </summary>
        string Identifier { get; set; }
        
        /// <summary>
        /// 密钥生成器。
        /// </summary>
        KeyGeneratorOptions KeyGenerator { get; set; }
    }
}
