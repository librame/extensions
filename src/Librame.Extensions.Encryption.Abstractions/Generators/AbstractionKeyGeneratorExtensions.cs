#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Generators
{
    using Core.Identifiers;

    /// <summary>
    /// 抽象密钥生成器静态扩展。
    /// </summary>
    public static class AbstractionKeyGeneratorExtensions
    {

        #region Symmetric Key

        /// <summary>
        /// 获取 AES 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetAesKey(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey256(identifier);

        /// <summary>
        /// 获取 DES 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetDesKey(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey64(identifier);

        /// <summary>
        /// 获取 TripleDES 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetTripleDesKey(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey192(identifier);

        #endregion


        #region HMAC Key

        /// <summary>
        /// 获取 HMAC MD5 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetHmacMd5Key(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey512(identifier);

        /// <summary>
        /// 获取 HMAC SHA1 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetHmacSha1Key(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey512(identifier);

        /// <summary>
        /// 获取 HMAC SHA256 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetHmacSha256Key(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey512(identifier);

        /// <summary>
        /// 获取 HMAC SHA384 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetHmacSha384Key(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey1024(identifier);

        /// <summary>
        /// 获取 HMAC SHA512 密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetHmacSha512Key(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.GetKey1024(identifier);

        #endregion


        #region Base Key

        /// <summary>
        /// 获取 64 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey64(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(8, identifier); // 64

        /// <summary>
        /// 获取 128 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey128(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(16, identifier); // 128

        /// <summary>
        /// 获取 192 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey192(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(24, identifier); // 192

        /// <summary>
        /// 获取 256 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey256(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(32, identifier); // 256

        /// <summary>
        /// 获取 384 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey384(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(48, identifier); // 384

        /// <summary>
        /// 获取 512 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey512(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(64, identifier); // 512

        /// <summary>
        /// 获取 1024 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey1024(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(128, identifier); // 1024

        /// <summary>
        /// 获取 2048 位密钥。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetKey2048(this IKeyGenerator keyGenerator, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateKey(256, identifier); // 2048

        #endregion

    }
}
