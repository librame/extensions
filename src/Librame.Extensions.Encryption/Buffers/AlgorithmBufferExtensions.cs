#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption.Buffers
{
    /// <summary>
    /// <see cref="IAlgorithmBuffer"/> 静态扩展。
    /// </summary>
    public static class AlgorithmBufferExtensions
    {
        /// <summary>
        /// 将明文来源转换为明文缓冲区。
        /// </summary>
        /// <param name="source">给定的明文字符串。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IPlaintextBuffer"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IPlaintextBuffer AsPlaintextBuffer(this string source, IServiceProvider serviceProvider)
            => new PlaintextBuffer(serviceProvider, source);

        /// <summary>
        /// 将缓冲区转换为算法缓冲区。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IAlgorithmBuffer"/>。</returns>
        public static IAlgorithmBuffer AsAlgorithmBuffer(this byte[] buffer, IServiceProvider serviceProvider)
            => new AlgorithmBuffer(serviceProvider, buffer);


        #region ToString

        /// <summary>
        /// 转换为 BASE32 字符串。
        /// </summary>
        /// <param name="algorithmBuffer">给定的 <see cref="IAlgorithmBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string AsBase32String(this IAlgorithmBuffer algorithmBuffer)
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));

            return algorithmBuffer.CurrentBuffer.ToArray().AsBase32String();
        }

        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="algorithmBuffer">给定的 <see cref="IAlgorithmBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string AsBase64String(this IAlgorithmBuffer algorithmBuffer)
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));

            return algorithmBuffer.CurrentBuffer.ToArray().AsBase64String();
        }

        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <param name="algorithmBuffer">给定的 <see cref="IAlgorithmBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string AsHexString(this IAlgorithmBuffer algorithmBuffer)
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));

            return algorithmBuffer.CurrentBuffer.ToArray().AsHexString();
        }


        /// <summary>
        /// 从 BASE32 字符串还原字节数组并转换为算法缓冲区。
        /// </summary>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IAlgorithmBuffer"/>。</returns>
        public static IAlgorithmBuffer FromBase32StringAsAlgorithmBuffer(this string base32String, IServiceProvider serviceProvider)
            => base32String.FromBase32String().AsAlgorithmBuffer(serviceProvider);

        /// <summary>
        /// 从 BASE64 字符串还原字节数组并转换为算法缓冲区。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IAlgorithmBuffer"/>。</returns>
        public static IAlgorithmBuffer FromBase64StringAsAlgorithmBuffer(this string base64String, IServiceProvider serviceProvider)
            => base64String.FromBase64String().AsAlgorithmBuffer(serviceProvider);

        /// <summary>
        /// 从 16 进制字符串还原字节数组并转换为算法缓冲区。
        /// </summary>
        /// <param name="base64String">给定的 16 进制字符串。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IAlgorithmBuffer"/>。</returns>
        public static IAlgorithmBuffer FromHexStringAsAlgorithmBuffer(this string base64String, IServiceProvider serviceProvider)
            => base64String.FromHexString().AsAlgorithmBuffer(serviceProvider);

        #endregion

    }
}
