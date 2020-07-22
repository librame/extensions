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

    /// <summary>
    /// <see cref="ISymmetricService"/> 静态扩展。
    /// </summary>
    public static class AbstractionSymmetricServiceExtensions
    {

        #region AES

        /// <summary>
        /// 加密 AES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncryptAes(this ISymmetricService service, byte[] buffer,
            SecurityToken token = null)
            => service.NotNull(nameof(service)).EncryptAes(buffer, out _, out _, token);

        /// <summary>
        /// 解密 AES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DecryptAes(this ISymmetricService service, byte[] buffer,
            SecurityToken token = null)
            => service.NotNull(nameof(service)).DecryptAes(buffer, out _, out _, token);

        #endregion


        #region DES

        /// <summary>
        /// 加密 DES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncryptDes(this ISymmetricService service, byte[] buffer,
            SecurityToken token = null)
            => service.NotNull(nameof(service)).EncryptDes(buffer, out _, out _, token);

        /// <summary>
        /// 解密 DES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DecryptDes(this ISymmetricService service, byte[] buffer,
            SecurityToken token = null)
            => service.NotNull(nameof(service)).DecryptDes(buffer, out _, out _, token);

        #endregion


        #region TripleDES

        /// <summary>
        /// 加密 TripleDES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncryptTripleDes(this ISymmetricService service, byte[] buffer,
            SecurityToken token = null)
            => service.NotNull(nameof(service)).EncryptTripleDes(buffer, out _, out _, token);

        /// <summary>
        /// 解密 TripleDES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="token">给定的 <see cref="SecurityToken"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DecryptTripleDes(this ISymmetricService service, byte[] buffer,
            SecurityToken token = null)
            => service.NotNull(nameof(service)).DecryptTripleDes(buffer, out _, out _, token);

        #endregion

    }
}
