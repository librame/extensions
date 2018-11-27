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
    /// 抽象字符缓冲区静态扩展。
    /// </summary>
    public static class AbstractCharBufferExtensions
    {

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <typeparam name="TCharBuffer">指定字符缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字符缓冲区。</param>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回字符缓冲区。</returns>
        public static TCharBuffer Change<TCharBuffer>(this TCharBuffer buffer, Func<Memory<char>, ReadOnlyMemory<char>> changeFactory)
            where TCharBuffer : ICharBuffer
        {
            return buffer.Change(changeFactory.Invoke(buffer.Memory));
        }
        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <typeparam name="TCharBuffer">指定字符缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字符缓冲区。</param>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{Char}"/>。</param>
        /// <returns>返回字符缓冲区。</returns>
        public static TCharBuffer Change<TCharBuffer>(this TCharBuffer buffer, ReadOnlyMemory<char> readOnlyMemory)
            where TCharBuffer : ICharBuffer
        {
            buffer.Memory = readOnlyMemory.ToArray();
            return buffer;
        }

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <typeparam name="TCharBuffer">指定字符缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字符缓冲区。</param>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回字符缓冲区。</returns>
        public static TCharBuffer Change<TCharBuffer>(this TCharBuffer buffer, Func<Memory<char>, Memory<char>> changeFactory)
            where TCharBuffer : ICharBuffer
        {
            return buffer.Change(changeFactory.Invoke(buffer.Memory));
        }
        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <typeparam name="TCharBuffer">指定字符缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字符缓冲区。</param>
        /// <param name="memory">给定的 <see cref="Memory{Char}"/>。</param>
        /// <returns>返回字符缓冲区。</returns>
        public static TCharBuffer Change<TCharBuffer>(this TCharBuffer buffer, Memory<char> memory)
            where TCharBuffer : ICharBuffer
        {
            buffer.Memory = memory;
            return buffer;
        }


        /// <summary>
        /// 清空存储器。
        /// </summary>
        /// <typeparam name="TCharBuffer">指定字符缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字符缓冲区。</param>
        /// <returns>返回字符缓冲区。</returns>
        public static TCharBuffer Clear<TCharBuffer>(this TCharBuffer buffer)
            where TCharBuffer : ICharBuffer
        {
            buffer.Memory = Memory<char>.Empty;
            return buffer;
        }


        #region SplitKeyValue

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByIndexOf(this ICharBuffer buffer, char separator)
        {
            return buffer.SplitKeyValueByIndexOf(separator, key => key.ToString(), value => value.ToString());
        }

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByIndexOf(this ICharBuffer buffer, string separator)
        {
            return buffer.SplitKeyValueByIndexOf(separator, key => key.ToString(), value => value.ToString());
        }


        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByLastIndexOf(this ICharBuffer buffer, char separator)
        {
            return buffer.SplitKeyValueByLastIndexOf(separator, key => key.ToString(), value => value.ToString());
        }

        /// <summary>
        /// 分拆为字符串键值对。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串键值对。</returns>
        public static KeyValuePair<string, string> SplitKeyValueStringByLastIndexOf(this ICharBuffer buffer, string separator)
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
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByIndexOf<TKey, TValue>(this ICharBuffer buffer, char separator,
            Func<Memory<char>, TKey> keyConverter, Func<Memory<char>, TValue> valueConverter)
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
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByIndexOf<TKey, TValue>(this ICharBuffer buffer, string separator,
            Func<Memory<char>, TKey> keyConverter, Func<Memory<char>, TValue> valueConverter)
        {
            var separatorIndex = buffer.GetString().IndexOf(separator);

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
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByLastIndexOf<TKey, TValue>(this ICharBuffer buffer, char separator,
            Func<Memory<char>, TKey> keyConverter, Func<Memory<char>, TValue> valueConverter)
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
        /// <param name="buffer">给定的 <see cref="ICharBuffer"/>。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <param name="keyConverter">给定的键转换器。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回键值对。</returns>
        public static KeyValuePair<TKey, TValue> SplitKeyValueByLastIndexOf<TKey, TValue>(this ICharBuffer buffer, string separator,
            Func<Memory<char>, TKey> keyConverter, Func<Memory<char>, TValue> valueConverter)
        {
            var separatorIndex = buffer.GetString().LastIndexOf(separator);

            return buffer.SplitKeyValue(separatorIndex, separator.Length, keyConverter, valueConverter);
        }


        private static KeyValuePair<TKey, TValue> SplitKeyValue<TKey, TValue>(this ICharBuffer buffer,
            int separatorIndex, int separatorLength,
            Func<Memory<char>, TKey> keyConverter, Func<Memory<char>, TValue> valueConverter)
        {
            if (separatorIndex < 0 || separatorIndex >= buffer.Memory.Length)
                return new KeyValuePair<TKey, TValue>(default, default);

            if (keyConverter == null) throw new ArgumentNullException(nameof(keyConverter));
            if (valueConverter == null) throw new ArgumentNullException(nameof(valueConverter));

            var key = buffer.Memory.Slice(0, separatorIndex);
            var value = buffer.Memory.Slice(separatorIndex + separatorLength);

            return new KeyValuePair<TKey, TValue>(keyConverter.Invoke(key), valueConverter.Invoke(value));
        }

        #endregion

    }
}
