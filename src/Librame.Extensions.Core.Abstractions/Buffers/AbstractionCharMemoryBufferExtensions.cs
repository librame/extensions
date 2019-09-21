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
using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象字符型可读写的连续内存缓冲区静态扩展。
    /// </summary>
    public static class AbstractionCharMemoryBufferExtensions
    {

        #region SplitKeyValue

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this ICharMemoryBuffer buffer, char separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByIndexOf(separator);

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this ICharMemoryBuffer buffer, string separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByIndexOf(separator);


        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this ICharMemoryBuffer buffer, char separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByLastIndexOf(separator);

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this ICharMemoryBuffer buffer, string separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByLastIndexOf(separator);


        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharReadOnlyMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this ICharReadOnlyMemoryBuffer buffer, char separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByIndexOf(separator);

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharReadOnlyMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this ICharReadOnlyMemoryBuffer buffer, string separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByIndexOf(separator);


        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharReadOnlyMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this ICharReadOnlyMemoryBuffer buffer, char separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByLastIndexOf(separator);

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharReadOnlyMemoryBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this ICharReadOnlyMemoryBuffer buffer, string separator)
            => buffer.NotNull(nameof(buffer)).Memory.Span.SplitKeyValueByLastIndexOf(separator);

        #endregion

    }
}
