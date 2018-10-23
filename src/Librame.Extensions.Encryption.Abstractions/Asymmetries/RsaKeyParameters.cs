#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    /// <summary>
    /// RSA 密钥参数。
    /// </summary>
    public class RsaKeyParameters
    {
        /// <summary>
        /// 密钥标识。
        /// </summary>
        public string KeyId { get; set; }
        
        /// <summary>
        /// RSA 参数。
        /// </summary>
        public RSAParameters Parameters { get; set; }


        /// <summary>
        /// 创建实例。
        /// </summary>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="keyId">给定的密钥标识（可选；默认内部新建）。</param>
        /// <returns>返回 <see cref="RsaKeyParameters"/>。</returns>
        public static RsaKeyParameters Create(RSAParameters parameters, string keyId = null)
        {
            if (string.IsNullOrEmpty(keyId))
                keyId = UniqueIdentifier.NewByGuid().ToString();

            return new RsaKeyParameters
            {
                KeyId = keyId,
                Parameters = parameters
            };
        }

    }
}
