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
    public class EncryptionBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 全局键名。
        /// </summary>
        public const string GLOBAL_KEY = "Global";


        /// <summary>
        /// 签名证书键名（默认使用全局键名）。
        /// </summary>
        public string SigningCredentialsKey { get; set; }
            = GLOBAL_KEY;

        /// <summary>
        /// 生成随机密钥（默认不随机生成）。
        /// </summary>
        public bool GenerateRandomKey { get; set; }
            = false;


        /// <summary>
        /// 标识符。
        /// </summary>
        public AlgorithmIdentifierOptions Identifier { get; set; }
            = new AlgorithmIdentifierOptions();
    }
}
