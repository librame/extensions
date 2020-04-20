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

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// 类型名组合器（不包含程序集名）。
    /// </summary>
    public class TypeNameCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 构造一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        public TypeNameCombiner(Type type)
            : this(type?.FullName)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <param name="fullName">给定的类型完整名（不包含程序集名）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public TypeNameCombiner(string fullName)
            : base(fullName)
        {
            var index = fullName.LastIndexOf('.');
            Namespace = fullName.Substring(0, index);
            Name = fullName.Substring(index + 1);
        }

        /// <summary>
        /// 构造一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <param name="namespace">给定的命名空间。</param>
        /// <param name="name">给定的名称。</param>
        public TypeNameCombiner(string @namespace, string name)
            : base(CombineString(@namespace, name))
        {
            Namespace = @namespace;
            Name = name;
        }


        /// <summary>
        /// 命名空间。
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineString(Namespace, Name);


        /// <summary>
        /// 改变命名空间。
        /// </summary>
        /// <param name="newNamespace">给定的新命名空间。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner ChangeNamespace(string newNamespace)
        {
            Namespace = newNamespace.NotEmpty(nameof(newNamespace));
            return this;
        }

        /// <summary>
        /// 改变名称。
        /// </summary>
        /// <param name="newName">给定的新名称。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner ChangeName(string newName)
        {
            Name = newName.NotEmpty(nameof(newName));
            return this;
        }


        /// <summary>
        /// 依据当前文件组合器的名称与指定的命名空间，新建一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <param name="newNamespace">给定的新命名空间。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner WithNamespace(string newNamespace)
            => new TypeNameCombiner(newNamespace, Name);

        /// <summary>
        /// 依据当前文件组合器的命名空间与指定的名称，新建一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <param name="newName">给定的新名称。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner WithName(string newName)
            => new TypeNameCombiner(Namespace, newName);


        /// <summary>
        /// 是否为指定的名称（忽略大小写）。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsName(string name)
            => Name.Equals(name, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的域名。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(string other)
            => Source.Equals(other, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is TypeNameCombiner other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source;


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="TypeNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="TypeNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(TypeNameCombiner a, TypeNameCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="TypeNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="TypeNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(TypeNameCombiner a, TypeNameCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="TypeNameCombiner"/>。</param>
        public static implicit operator string(TypeNameCombiner combiner)
            => combiner?.ToString();


        /// <summary>
        /// 组合字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="namespace"/> or <paramref name="name"/> is null or empty.
        /// </exception>
        /// <param name="namespace">给定的命名空间。</param>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineString(string @namespace, string name)
            => $"{@namespace.NotEmpty(nameof(@namespace))}.{name.NotEmpty(nameof(name))}";
    }
}
