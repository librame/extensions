#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.RsaKeySerializers
{
    /// <summary>
    /// RSA 密钥序列化器接口。
    /// </summary>
    public interface IRsaKeySerializer : IEncryptionService
    {
        /// <summary>
        /// 文件扩展名。
        /// </summary>
        string FileExtension { get; }


        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="fileName">给定要读取的文件名。</param>
        /// <returns>返回 <see cref="RsaKeyParameters"/>。</returns>
        RsaKeyParameters Deserialize(string fileName);

        /// <summary>
        /// 序列化密钥参数。
        /// </summary>
        /// <param name="keyParameters">给定的 <see cref="RsaKeyParameters"/>。</param>
        /// <param name="fileName">给定要保存的文件名。</param>
        /// <returns>返回序列化字符串。</returns>
        string Serialize(RsaKeyParameters keyParameters, string fileName);
    }
}
