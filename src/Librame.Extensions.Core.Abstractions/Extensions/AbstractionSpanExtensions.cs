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
    /// 抽象一段连续的内存静态扩展。
    /// </summary>
    public static class AbstractionSpanExtensions
    {

        #region SplitKeyValue

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this ReadOnlySpan<char> buffer, char separator)
            => buffer.SplitKeyValueByIndexOf(separator.ToString());

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this ReadOnlySpan<char> buffer, string separator)
        {
            var separatorIndex = buffer.ToString().IndexOf(separator);
            return buffer.SplitKeyValue(separatorIndex, separator.Length);
        }


        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this ReadOnlySpan<char> buffer, char separator)
            => buffer.SplitKeyValueByLastIndexOf(separator.ToString());

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this ReadOnlySpan<char> buffer, string separator)
        {
            var separatorIndex = buffer.ToString().LastIndexOf(separator);
            return buffer.SplitKeyValue(separatorIndex, separator.Length);
        }

        private static KeyValuePair<string, string> SplitKeyValue(this ReadOnlySpan<char> span,
            int separatorIndex, int separatorLength)
        {
            if (separatorIndex < 0 || separatorIndex >= span.Length)
                return default;

            var key = span.Slice(0, separatorIndex).ToString();
            var value = span.Slice(separatorIndex + separatorLength).ToString();

            return new KeyValuePair<string, string>(key, value);
        }


        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="Memory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this Span<char> buffer, char separator)
            => buffer.SplitKeyValueByIndexOf(separator.ToString());

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="Memory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByIndexOf(this Span<char> buffer, string separator)
        {
            var separatorIndex = buffer.ToString().IndexOf(separator);
            return buffer.SplitKeyValue(separatorIndex, separator.Length);
        }


        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="Memory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this Span<char> buffer, char separator)
            => buffer.SplitKeyValueByLastIndexOf(separator.ToString());

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="Memory{Char}"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueByLastIndexOf(this Span<char> buffer, string separator)
        {
            var separatorIndex = buffer.ToString().LastIndexOf(separator);
            return buffer.SplitKeyValue(separatorIndex, separator.Length);
        }

        private static KeyValuePair<string, string> SplitKeyValue(this Span<char> span,
            int separatorIndex, int separatorLength)
        {
            if (separatorIndex < 0 || separatorIndex >= span.Length)
                return default;

            var key = span.Slice(0, separatorIndex).ToString();
            var value = span.Slice(separatorIndex + separatorLength).ToString();

            return new KeyValuePair<string, string>(key, value);
        }

        #endregion

    }
}
