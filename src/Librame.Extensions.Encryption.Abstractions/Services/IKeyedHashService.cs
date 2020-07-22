#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Services
{
    using Core.Tokens;
    using Core.Services;
    using Encryption.Generators;

    /// <summary>
    /// 键控散列服务接口。
    /// </summary>
    public interface IKeyedHashService : IService
    {
        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IKeyGenerator"/>。
        /// </value>
        IKeyGenerator KeyGenerator { get; }


        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] HmacMd5(byte[] buffer, out byte[] key, SecurityToken token = null);

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] HmacSha1(byte[] buffer, out byte[] key, SecurityToken token = null);

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] HmacSha256(byte[] buffer, out byte[] key, SecurityToken token = null);

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] HmacSha384(byte[] buffer, out byte[] key, SecurityToken token = null);

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">输出密钥。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] HmacSha512(byte[] buffer, out byte[] key, SecurityToken token = null);
    }
}
