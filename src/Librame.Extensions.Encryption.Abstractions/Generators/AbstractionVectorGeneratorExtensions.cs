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
    /// 抽象向量生成器静态扩展。
    /// </summary>
    public static class AbstractionVectorGeneratorExtensions
    {

        #region Symmetric Vector

        /// <summary>
        /// 获取 AES 向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetAesVector(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.GetVector128(key, identifier);

        /// <summary>
        /// 获取 DES 向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetDesVector(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.GetVector64(key, identifier);

        /// <summary>
        /// 获取 TripleDES 向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetTripleDesVector(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.GetVector64(key, identifier);

        #endregion


        #region Base Vector

        /// <summary>
        /// 获取 64 位向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetVector64(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateVector(key, 8, identifier); // 64

        /// <summary>
        /// 获取 128 位向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetVector128(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateVector(key, 16, identifier); // 128

        /// <summary>
        /// 获取 192 位向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetVector192(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateVector(key, 24, identifier); // 192

        /// <summary>
        /// 获取 256 位向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetVector256(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateVector(key, 32, identifier); // 256

        /// <summary>
        /// 获取 384 位向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetVector384(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateVector(key, 48, identifier); // 384

        /// <summary>
        /// 获取 512 位向量。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IVectorGenerator"/>。</param>
        /// <param name="key">给定的密钥。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GetVector512(this IVectorGenerator keyGenerator,
            byte[] key, SecurityIdentifier identifier = null)
            => keyGenerator.NotNull(nameof(keyGenerator)).GenerateVector(key, 64, identifier); // 512

        #endregion

    }
}
