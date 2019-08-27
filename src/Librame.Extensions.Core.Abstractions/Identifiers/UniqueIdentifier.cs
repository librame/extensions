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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 唯一标识符。
    /// </summary>
    public struct UniqueIdentifier : IIdentifier<byte>, IEquatable<UniqueIdentifier>
    {
        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/>。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/> 。</param>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter{Byte}"/>（可选；默认使用 <see cref="HexIdentifierConverter"/>）。</param>
        public UniqueIdentifier(Guid guid, IIdentifierConverter<byte> converter = null)
        {
            Memory = guid.ToByteArray();
            Converter = converter ?? new HexIdentifierConverter();
            RawGuid = guid;
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定标识符的字符串形式。</param>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter{Byte}"/>。</param>
        public UniqueIdentifier(string identifier, IIdentifierConverter<byte> converter)
        {
            Converter = converter.NotNull(nameof(converter));
            Memory = Converter.From(identifier);
            RawGuid = new Guid(Memory.ToArray());
        }


        /// <summary>
        /// 只读存储器。
        /// </summary>
        public ReadOnlyMemory<byte> Memory { get; }

        /// <summary>
        /// 转换器。
        /// </summary>
        public IIdentifierConverter<byte> Converter { get; }

        /// <summary>
        /// 原始 GUID。
        /// </summary>
        public Guid RawGuid { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(UniqueIdentifier other)
        {
            return Memory.ToArray().SequenceEqual(other.Memory.ToArray());
            //return Memory.Equals(other.Memory);
        }


        /// <summary>
        /// 使用有顺序的 GUID 与当前转换器，构造一个新唯一标识符。
        /// </summary>
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public UniqueIdentifier ToCombUniqueIdentifier()
        {
            return new UniqueIdentifier(RawGuid.AsCombId(), Converter);
        }

        /// <summary>
        /// 转换为短字符串（可用作排序）。
        /// </summary>
        /// <param name="toUpper">转换为大写形式（可选；默认启用转换）。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(bool toUpper = true)
        {
            var i = 1L;

            foreach (var b in Memory.ToArray())
                i *= b + 1;

            // 8d7225f69933e15
            var str = string.Format("{0:x}", _ = DateTimeOffset.UtcNow.Ticks);

            if (toUpper)
                return str.ToUpperInvariant();

            return str;
        }

        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Converter.To(Memory);
        }


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <param name="obj">给定的标识符对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is UniqueIdentifier identifier)
                return Equals(identifier);

            if (obj is ReadOnlyMemory<byte> memory)
                return Memory.Equals(memory);

            return false;
        }

        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
            //return Memory.GetHashCode();
        }

        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(UniqueIdentifier a, UniqueIdentifier b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="UniqueIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(UniqueIdentifier a, UniqueIdentifier b)
        {
            return !a.Equals(b);
        }


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="UniqueIdentifier"/>。</param>
        public static implicit operator string(UniqueIdentifier identifier)
            => identifier.ToString();


        /// <summary>
        /// 只读空实例。
        /// </summary>
        public static readonly UniqueIdentifier Empty
            = new UniqueIdentifier(Guid.Empty);


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter{Byte}"/>（可选；默认使用 <see cref="HexIdentifierConverter"/>）。</param>
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public static UniqueIdentifier New(IIdentifierConverter<byte> converter = null)
            => new UniqueIdentifier(Guid.NewGuid(), converter);

        /// <summary>
        /// 新建数组实例。
        /// </summary>
        /// <param name="count">给定要生成的实例数量。</param>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter{Byte}"/>（可选；默认使用 <see cref="HexIdentifierConverter"/>）。</param>
        /// <returns>返回 <see cref="UniqueIdentifier"/> 数组。</returns>
        public static UniqueIdentifier[] NewArray(int count, IIdentifierConverter<byte> converter = null)
        {
            var identifiers = new UniqueIdentifier[count];

            for (var i = 0; i < count; i++)
                identifiers[i] = New(converter);

            return identifiers;
        }

    }
}
