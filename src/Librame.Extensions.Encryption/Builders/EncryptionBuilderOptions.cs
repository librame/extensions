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
    using Core.Serializers;
    using Core.Tokens;

    /// <summary>
    /// 加密构建器选项。
    /// </summary>
    public class EncryptionBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 全局签名证书键名。
        /// </summary>
        public const string GlobalSigningCredentialsKey = "Global";


        /// <summary>
        /// 签名证书键名（默认使用全局键名）。
        /// </summary>
        public string SigningCredentialsKey { get; set; }
            = GlobalSigningCredentialsKey;

        /// <summary>
        /// 生成随机密钥（默认不随机生成；注：如果启用此选项，请自行保存密钥，否则会无法正确解密）。
        /// </summary>
        public bool GenerateRandomKey { get; set; }
            = false;

        /// <summary>
        /// 生成随机向量（默认不随机生成；注：如果启用此选项，请自行保存向量，否则会无法正确解密）。
        /// </summary>
        public bool GenerateRandomVector { get; set; }
            = false;


        /// <summary>
        /// 安全令牌。
        /// </summary>
        public SerializableString<SecurityToken> Token { get; set; }
            = new SerializableString<SecurityToken>(SecurityToken.New());
    }
}
