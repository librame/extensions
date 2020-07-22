#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Generators
{
    using Core.Tokens;
    using Core.Services;

    /// <summary>
    /// 向量生成器接口。
    /// </summary>
    public interface IVectorGenerator : IService
    {
        /// <summary>
        /// 生成向量。
        /// </summary>
        /// <param name="key">给定的密钥。</param>
        /// <param name="length">给定要生成的向量长度。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] GenerateVector(byte[] key, int length, SecurityToken token = null);
    }
}
