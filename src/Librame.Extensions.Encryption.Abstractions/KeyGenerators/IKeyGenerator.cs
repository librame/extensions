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
    using Buffers;

    /// <summary>
    /// 密钥生成器接口。
    /// </summary>
    public interface IKeyGenerator : IEncryptionService
    {
        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="length">给定要生成的密钥长度。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        IByteBuffer GenerateKey(int length, string identifier = null);
    }
}
