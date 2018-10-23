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
    /// <summary>
    /// 默认加密构建器选项。
    /// </summary>
    public class DefaultEncryptionBuilderOptions : IEncryptionBuilderOptions
    {
        /// <summary>
        /// 标识符（默认新建标识符；参考 <see cref="AlgorithmIdentifier"/>）。
        /// </summary>
        public string Identifier { get; set; } = AlgorithmIdentifier.New().ToString();


        /// <summary>
        /// 密钥生成器。
        /// </summary>
        public KeyGeneratorOptions KeyGenerator { get; set; }
            = new KeyGeneratorOptions();
    }


    /// <summary>
    /// 密钥生成器选项。
    /// </summary>
    public class KeyGeneratorOptions
    {
        /// <summary>
        /// 是否生成随机密钥（默认不生成）。
        /// </summary>
        public bool IsRandomKey { get; set; } = false;
    }

}
