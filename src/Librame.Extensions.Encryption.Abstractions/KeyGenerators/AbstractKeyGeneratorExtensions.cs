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
    /// 抽象密钥生成器静态扩展。
    /// </summary>
    public static class AbstractKeyGeneratorExtensions
    {

        /// <summary>
        /// 获取 64 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey64(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(8, identifier); // 64
        }

        /// <summary>
        /// 获取 128 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey128(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(16, identifier); // 128
        }

        /// <summary>
        /// 获取 192 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey192(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(24, identifier); // 192
        }

        /// <summary>
        /// 获取 256 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey256(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(32, identifier); // 256
        }

        /// <summary>
        /// 获取 384 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey384(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(48, identifier); // 384
        }

        /// <summary>
        /// 获取 512 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey512(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(64, identifier); // 512
        }

        /// <summary>
        /// 获取 1024 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey1024(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(128, identifier); // 1024
        }

        /// <summary>
        /// 获取 2048 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer GetKey2048(this IKeyGenerator keyGenerator, string identifier = null)
        {
            return keyGenerator.GenerateKey(256, identifier); // 2048
        }

    }
}
