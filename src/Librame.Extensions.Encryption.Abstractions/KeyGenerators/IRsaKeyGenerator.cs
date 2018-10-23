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
    /// RSA 密钥生成器接口。
    /// </summary>
    public interface IRsaKeyGenerator : IEncryptionService
    {
        /// <summary>
        /// 生成密钥参数。
        /// </summary>
        /// <param name="rsaKeyFileName">给定的 RSA 密钥文件名（可选；默认不使用）。</param>
        /// <param name="forceRegen">强制重新生成（可选；默认不强制）。</param>
        /// <returns>返回 <see cref="RsaKeyParameters"/>。</returns>
        RsaKeyParameters GenerateKeyParameters(string rsaKeyFileName = null, bool forceRegen = false);
    }
}
