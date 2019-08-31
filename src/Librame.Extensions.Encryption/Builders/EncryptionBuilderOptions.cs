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
    public class EncryptionBuilderOptions : AbstractExtensionBuilderOptions
    {
        private static readonly UniqueIdentifier _defaultIdentifier
            = UniqueIdentifier.Empty;

        /// <summary>
        /// 全局键名。
        /// </summary>
        public const string GLOBAL_KEY = "Global";


        /// <summary>
        /// 标识符。
        /// </summary>
        public string Identifier { get; set; }
            = _defaultIdentifier;

        /// <summary>
        /// 标识符转换器。
        /// </summary>
        public IIdentifierConverter<byte> IdentifierConverter { get; set; }
            = _defaultIdentifier.Converter;


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
        /// 是否生成随机密钥（默认不随机生成）。
        /// </summary>
        public bool IsRandomKey { get; set; }
            = false;
    }

}
