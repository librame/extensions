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
    using Core.Services;
    using Core.Tokens;

    /// <summary>
    /// 密钥生成器接口。
    /// </summary>
    public interface IKeyGenerator : IService
    {
        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="length">给定要生成的密钥长度。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] GenerateKey(int length, SecurityToken token = null);
    }
}
