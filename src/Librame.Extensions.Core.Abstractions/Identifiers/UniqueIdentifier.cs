﻿#region License

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
    public struct UniqueIdentifier : IIdentifier, IEquatable<UniqueIdentifier>
    {
        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/> 。</param>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter"/>（可选）。</param>
        public UniqueIdentifier(Guid guid, IIdentifierConverter converter = null)
            : this(guid.ToByteArray(), converter)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter"/>（可选）。</param>
        public UniqueIdentifier(ReadOnlyMemory<byte> memory, IIdentifierConverter converter = null)
        {
            Memory = memory;
            Converter = converter ?? new Base64IdentifierConverter();
        }

        /// <summary>
        /// 构造一个 <see cref="UniqueIdentifier"/> 实例。
        /// </summary>
        /// <param name="identifier">给定标识符的字符串形式。</param>
        /// <param name="converter">给定的 <see cref="IIdentifierConverter"/>（可选）。</param>
        public UniqueIdentifier(string identifier, IIdentifierConverter converter = null)
        {
            Converter = converter ?? new Base64IdentifierConverter();
            Memory = Converter.From(identifier);
        }


        /// <summary>
        /// 只读存储器。
        /// </summary>
        public ReadOnlyMemory<byte> Memory { get; }

        /// <summary>
        /// 转换器。
        /// </summary>
        public IIdentifierConverter Converter { get; set; }


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
        /// 显式转换为 <see cref="UniqueIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定标识符的字符串形式。</param>
        public static explicit operator UniqueIdentifier(string identifier)
            => new UniqueIdentifier(identifier);

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
        /// <returns>返回 <see cref="UniqueIdentifier"/>。</returns>
        public static UniqueIdentifier New()
            => new UniqueIdentifier(Guid.NewGuid());
    }
}