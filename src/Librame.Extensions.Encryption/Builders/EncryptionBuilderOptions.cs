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
    using Core;

    /// <summary>
    /// 加密构建器选项。
    /// </summary>
    public class EncryptionBuilderOptions : AbstractBuilderOptions, IBuilderOptions
    {
        /// <summary>
        /// 全局键名。
        /// </summary>
        public const string GLOBAL_KEY = "Global";


        /// <summary>
        /// 标识符（默认新建标识符；参考 <see cref="AlgorithmIdentifier"/>）。
        /// </summary>
        public string Identifier { get; set; }
            = AlgorithmIdentifier.Empty.ToString();

        /// <summary>
        /// 签名证书键名（默认使用全局键名）。
        /// </summary>
        public string SigningCredentialsKey { get; set; }
            = GLOBAL_KEY;


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
        public bool IsRandomKey { get; set; }
            = false;
    }

}
