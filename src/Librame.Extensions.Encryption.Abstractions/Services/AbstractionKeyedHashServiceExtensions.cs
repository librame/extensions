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
    using Core.Identifiers;

    /// <summary>
    /// <see cref="IKeyedHashService"/> 静态扩展。
    /// </summary>
    public static class AbstractionKeyedHashServiceExtensions
    {
        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="service">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacMd5(this IKeyedHashService service, byte[] buffer, SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).HmacMd5(buffer, out _, identifier);

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="service">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha1(this IKeyedHashService service, byte[] buffer, SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).HmacMd5(buffer, out _, identifier);

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="service">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha256(this IKeyedHashService service, byte[] buffer, SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).HmacMd5(buffer, out _, identifier);

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="service">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha384(this IKeyedHashService service, byte[] buffer, SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).HmacMd5(buffer, out _, identifier);

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="service">给定的 <see cref="IKeyedHashService"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha512(this IKeyedHashService service, byte[] buffer, SecurityIdentifier identifier = null)
            => service.NotNull(nameof(service)).HmacMd5(buffer, out _, identifier);
    }
}
