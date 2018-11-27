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

namespace Librame.Extensions
{
    using Buffers;

    /// <summary>
    /// 抽象只读字符缓冲区静态扩展。
    /// </summary>
    public static class AbstractReadOnlyCharBufferExtensions
    {

        #region SplitKeyValue

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByIndexOf(this IReadOnlyCharBuffer buffer, char separator)
        {
            return buffer.SplitKeyValueByIndexOf(separator, key => key.ToString(), value => value.ToString());
        }

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByIndexOf(this IReadOnlyCharBuffer buffer, string separator)
        {
            return buffer.SplitKeyValueByIndexOf(separator, key => key.ToString(), value => value.ToString());
        }


        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByLastIndexOf(this IReadOnlyCharBuffer buffer, char separator)
        {
            return buffer.SplitKeyValueByLastIndexOf(separator, key => key.ToString(), value => value.ToString());
        }

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByLastIndexOf(this IReadOnlyCharBuffer buffer, string separator)
        {
            return buffer.SplitKeyValueByLastIndexOf(separator, key => key.ToString(), value => value.ToString());
        }


        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// keyConverter or valueConverter is null.
        /// </exception>
        /// <typeparam name="TKey">指定的键类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByIndexOf<TKey, TValue>(this IReadOnlyCharBuffer buffer, char separator,
            Func<ReadOnlyMemory<char>, TKey> keyConverter, Func<ReadOnlyMemory<char>, TValue> valueConverter)
        {
            return buffer.SplitKeyValueByIndexOf(separator.ToString(), keyConverter, valueConverter);
        }

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// keyConverter or valueConverter is null.
        /// </exception>
        /// <typeparam name="TKey">指定的键类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByIndexOf<TKey, TValue>(this IReadOnlyCharBuffer buffer, string separator,
            Func<ReadOnlyMemory<char>, TKey> keyConverter, Func<ReadOnlyMemory<char>, TValue> valueConverter)
        {
            var separatorIndex = buffer.GetReadOnlyString().IndexOf(separator);

            return buffer.SplitKeyValue(separatorIndex, separator.Length, keyConverter, valueConverter);
        }


        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// keyConverter or valueConverter is null.
        /// </exception>
        /// <typeparam name="TKey">指定的键类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByLastIndexOf<TKey, TValue>(this IReadOnlyCharBuffer buffer, char separator,
            Func<ReadOnlyMemory<char>, TKey> keyConverter, Func<ReadOnlyMemory<char>, TValue> valueConverter)
        {
            return buffer.SplitKeyValueByLastIndexOf(separator.ToString(), keyConverter, valueConverter);
        }

        /// <summary>
        /// 分拆为键值对。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// keyConverter or valueConverter is null.
        /// </exception>
        /// <typeparam name="TKey">指定的键类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IReadOnlyCharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByLastIndexOf<TKey, TValue>(this IReadOnlyCharBuffer buffer, string separator,
            Func<ReadOnlyMemory<char>, TKey> keyConverter, Func<ReadOnlyMemory<char>, TValue> valueConverter)
        {
            var separatorIndex = buffer.GetReadOnlyString().LastIndexOf(separator);

            return buffer.SplitKeyValue(separatorIndex, separator.Length, keyConverter, valueConverter);
        }


        private static KeyValuePair<TKey, TValue> SplitKeyValue<TKey, TValue>(this IReadOnlyCharBuffer buffer,
            int separatorIndex, int separatorLength,
            Func<ReadOnlyMemory<char>, TKey> keyConverter, Func<ReadOnlyMemory<char>, TValue> valueConverter)
        {
            if (separatorIndex < 0 || separatorIndex >= buffer.ReadOnlyMemory.Length)
                return new KeyValuePair<TKey, TValue>(default, default);

            if (keyConverter == null) throw new ArgumentNullException(nameof(keyConverter));
            if (valueConverter == null) throw new ArgumentNullException(nameof(valueConverter));

            var key = buffer.ReadOnlyMemory.Slice(0, separatorIndex);
            var value = buffer.ReadOnlyMemory.Slice(separatorIndex + separatorLength);

            return new KeyValuePair<TKey, TValue>(keyConverter.Invoke(key), valueConverter.Invoke(value));
        }

        #endregion

    }
}
