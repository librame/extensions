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
using System.Globalization;
using System.Linq;

namespace Librame.Extensions.Core.Identifiers
{
    using Serializers;

    /// <summary>
    /// 抽象算法标识符。
    /// </summary>
    public abstract class AbstractAlgorithmIdentifier : IAlgorithmIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="SerializableObject{ReadOnlyMemory}"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "readOnlyMemory")]
        public AbstractAlgorithmIdentifier(SerializableObject<ReadOnlyMemory<byte>> readOnlyMemory)
        {
            ReadOnlyMemory = readOnlyMemory.NotNull(nameof(readOnlyMemory));
        }


        /// <summary>
        /// 只读内存。
        /// </summary>
        public SerializableObject<ReadOnlyMemory<byte>> ReadOnlyMemory { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IAlgorithmIdentifier other)
            => ReadOnlyMemory.Source.ToArray().SequenceEqual(other?.ReadOnlyMemory.Source.ToArray());

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is IAlgorithmIdentifier other) ? Equals(other) : false;


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => ReadOnlyMemory.Value;


        /// <summary>
        /// 转换为短字符串。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ToShortString(DateTimeOffset timestamp)
        {
            var i = 1L;
            foreach (var b in ReadOnlyMemory.Source.ToArray())
                i *= b + 1;

            // Length(15): 8d737ebe809e70e
            return string.Format(CultureInfo.InvariantCulture, "{0:x}", _ = timestamp.Ticks);
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractAlgorithmIdentifier a, AbstractAlgorithmIdentifier b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractAlgorithmIdentifier a, AbstractAlgorithmIdentifier b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        public static implicit operator string(AbstractAlgorithmIdentifier identifier)
            => identifier?.ToString();
    }
}
