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
using System.Linq;
using System.Runtime.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象算法标识符。
    /// </summary>
    [Serializable]
    public abstract class AbstractAlgorithmIdentifier : IAlgorithmIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public AbstractAlgorithmIdentifier(ReadOnlyMemory<byte> memory, IAlgorithmConverter converter)
        {
            Memory = memory;
            Converter = converter.NotNull(nameof(converter));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的算法字符串。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public AbstractAlgorithmIdentifier(string identifier, IAlgorithmConverter converter)
        {
            Converter = converter.NotNull(nameof(converter));
            Memory = converter.From(identifier);
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="SerializationInfo"/>。</param>
        /// <param name="context">给定的 <see cref="StreamingContext"/>。</param>
        public AbstractAlgorithmIdentifier(SerializationInfo info, StreamingContext context)
        {
            var converterType = Type.GetType(info.GetString("ConverterTypeName"));
            Converter = (IAlgorithmConverter)info.GetValue("Converter", converterType);

            var identifier = info.GetString("Identifier");
            Memory = Converter.From(identifier);
        }


        /// <summary>
        /// 只读的连续内存区域。
        /// </summary>
        public ReadOnlyMemory<byte> Memory { get; }

        /// <summary>
        /// 算法转换器。
        /// </summary>
        public IAlgorithmConverter Converter { get; }


        /// <summary>
        /// 获取对象数据。
        /// </summary>
        /// <param name="info">给定的 <see cref="SerializationInfo"/>。</param>
        /// <param name="context">给定的 <see cref="StreamingContext"/>。</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Identifier", Converter.To(Memory));
            info.AddValue("Converter", Converter);
            info.AddValue("ConverterTypeName", Converter.GetType().ToString());
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IAlgorithmIdentifier other)
            => Memory.ToArray().SequenceEqual(other?.Memory.ToArray());

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
            => ToString().GetHashCode();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Converter.To(Memory);


        /// <summary>
        /// 转换为短字符串。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回字符串。</returns>
        public virtual string ToShortString(DateTimeOffset timestamp)
        {
            var i = 1L;
            foreach (var b in Memory.ToArray())
                i *= b + 1;

            // Length(15): 8d737ebe809e70e
            return string.Format("{0:x}", _ = timestamp.Ticks);
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractAlgorithmIdentifier a, AbstractAlgorithmIdentifier b)
            => a.Equals(b);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractAlgorithmIdentifier a, AbstractAlgorithmIdentifier b)
            => !a.Equals(b);
    }
}
