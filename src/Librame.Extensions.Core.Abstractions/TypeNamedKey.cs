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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 类型命名键。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class TypeNamedKey<T> : TypeNamedKey
    {
        /// <summary>
        /// 构造一个 <see cref="TypeNamedKey{T}"/>。
        /// </summary>
        public TypeNamedKey()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TypeNamedKey{T}"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        public TypeNamedKey(string name)
            : base(name, typeof(T))
        {
        }
    }


    /// <summary>
    /// 类型命名键。
    /// </summary>
    public class TypeNamedKey : IEquatable<TypeNamedKey>
    {
        /// <summary>
        /// 构造一个 <see cref="TypeNamedKey"/>。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        public TypeNamedKey(Type type)
            : this(type?.Name, type)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TypeNamedKey"/>。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="name">给定的名称。</param>
        public TypeNamedKey(string name, Type type = null)
        {
            Name = name.NotEmpty(nameof(name));
            Type = type;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 类型。
        /// </summary>
        public Type Type { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(TypeNamedKey other)
            => ToString() == other?.ToString();

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回的布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is TypeNamedKey other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Name;
    }
}
