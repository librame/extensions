#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Encryption.Services
{
    using Core.Identifiers;

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
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncryptAes(this ISymmetricService service, byte[] buffer,
            SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).EncryptAes(buffer, out _, out _, identifier);

        /// <summary>
        /// 解密 AES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DecryptAes(this ISymmetricService service, byte[] buffer,
            SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).DecryptAes(buffer, out _, out _, identifier);

        #endregion


        #region DES

        /// <summary>
        /// 加密 DES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncryptDes(this ISymmetricService service, byte[] buffer,
            SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).EncryptDes(buffer, out _, out _, identifier);

        /// <summary>
        /// 解密 DES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DecryptDes(this ISymmetricService service, byte[] buffer,
            SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).DecryptDes(buffer, out _, out _, identifier);

        #endregion


        #region TripleDES

        /// <summary>
        /// 加密 TripleDES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncryptTripleDes(this ISymmetricService service, byte[] buffer,
            SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).EncryptTripleDes(buffer, out _, out _, identifier);

        /// <summary>
        /// 解密 TripleDES。
        /// </summary>
        /// <param name="service">给定的 <see cref="ISymmetricService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DecryptTripleDes(this ISymmetricService service, byte[] buffer,
            SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).DecryptTripleDes(buffer, out _, out _, identifier);

        #endregion

    }
}
