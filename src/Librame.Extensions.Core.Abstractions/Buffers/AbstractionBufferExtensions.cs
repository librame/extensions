#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象缓冲区静态扩展。
    /// </summary>
    public static class AbstractionBufferExtensions
    {
        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this IByteMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray().AsBase64String();

        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this IByteMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray().AsHexString();
    }
}
