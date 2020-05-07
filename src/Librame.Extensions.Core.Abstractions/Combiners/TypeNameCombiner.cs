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
using System.Reflection;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// 类型名称组合器。
    /// </summary>
    public class TypeNameCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 构造一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        /// <param name="type">给定的类型。</param>
        public TypeNameCombiner(Type type)
            : this(type?.ToString())
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeString"/> is null or empty.
        /// </exception>
        /// <param name="typeString">给定的类型字符串（支持仅名称、带命名空间、包含程序集等）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public TypeNameCombiner(string typeString)
            : base(typeString)
        {
            if (!TryParseParameters(typeString, out var name, out var @namespace, out var assembly))
                throw new ArgumentException($"Invalid type string '{typeString}'");

            Name = name;
            Namespace = @namespace;
            Assembly = assembly;
        }

        /// <summary>
        /// 构造一个 <see cref="TypeNameCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> is null or empty.
        /// </exception>
        /// <param name="name">给定的名称。</param>
        /// <param name="namespace">给定的命名空间（可空）。</param>
        /// <param name="assembly">给定的程序集（可空）。</param>
        public TypeNameCombiner(string name, string @namespace, string assembly = null)
            : base(CombineParameters(name, @namespace, assembly))
        {
            Name = name;
            Namespace = @namespace;
            Assembly = assembly;
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
        /// 程序集。
        /// </summary>
        public string Assembly { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineParameters(Name, Namespace, Assembly);


        #region Change

        /// <summary>
        /// 改变名称。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newName"/> is null or empty.
        /// </exception>
        /// <param name="newName">给定的新名称。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner ChangeName(string newName)
        {
            Name = newName.NotEmpty(nameof(newName));
            return this;
        }

        /// <summary>
        /// 改变命名空间。
        /// </summary>
        /// <param name="newNamespace">给定的新命名空间。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner ChangeNamespace(string newNamespace)
        {
            Namespace = newNamespace;
            return this;
        }

        /// <summary>
        /// 改变程序集。
        /// </summary>
        /// <param name="newAssembly">给定的新程序集。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner ChangeAssembly(string newAssembly)
        {
            Assembly = newAssembly;
            return this;
        }

        #endregion


        #region With

        /// <summary>
        /// 带新名称的新实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="newName"/> is null or empty.
        /// </exception>
        /// <param name="newName">给定的新名称。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner WithName(string newName)
            => new TypeNameCombiner(newName, Namespace, Assembly);

        /// <summary>
        /// 带新命名空间的新实例。
        /// </summary>
        /// <param name="newNamespace">给定的新命名空间。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner WithNamespace(string newNamespace)
            => new TypeNameCombiner(Name, newNamespace, Assembly);

        /// <summary>
        /// 带新程序集的新实例。
        /// </summary>
        /// <param name="newAssembly">给定的新程序集。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public TypeNameCombiner WithAssembly(string newAssembly)
            => new TypeNameCombiner(Name, Namespace, newAssembly);

        #endregion


        /// <summary>
        /// 是否为指定的名称（忽略大小写）。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsName(string name)
            => Name.Equals(name, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 创建实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的构造参数集合。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public T Create<T>(params object[] parameters)
        {
            if (parameters.IsNotEmpty())
                return ToType().EnsureCreate<T>(parameters);

            return ToType().EnsureCreate<T>();
        }

        /// <summary>
        /// 创建对象。
        /// </summary>
        /// <param name="parameters">给定的构造参数集合。</param>
        /// <returns>返回对象。</returns>
        public object CreateObject(params object[] parameters)
        {
            if (parameters.IsNotEmpty())
                return ToType().EnsureCreateObject(parameters);

            return ToType().EnsureCreateObject();
        }


        /// <summary>
        /// 转换为类型。
        /// </summary>
        /// <returns>返回 <see cref="Type"/>。</returns>
        public Type ToType()
            => Type.GetType(Source);

        /// <summary>
        /// 转换为类型信息。
        /// </summary>
        /// <returns>返回 <see cref="TypeInfo"/>。</returns>
        public TypeInfo ToTypeInfo()
            => ToType().GetTypeInfo();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public string ToShortString()
            => Name;


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
        /// 隐式转换为类型。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="TypeNameCombiner"/>。</param>
        public static implicit operator Type(TypeNameCombiner combiner)
            => combiner?.ToType();

        /// <summary>
        /// 隐式转换为类型信息。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="TypeNameCombiner"/>。</param>
        public static implicit operator TypeInfo(TypeNameCombiner combiner)
            => combiner?.ToTypeInfo();


        private static string CombineParameters(string name,
            string @namespace = null, string assembly = null)
        {
            name.NotEmpty(nameof(name));

            if (@namespace.IsNotEmpty())
                name = $"{@namespace}.{name}";

            if (assembly.IsNotEmpty())
                return $"{name}, {assembly}";

            return name;
        }

        private static bool TryParseParameters(string typeString,
            out string name, out string @namespace, out string assembly)
        {
            typeString.NotEmpty(nameof(typeString));

            if (typeString.CompatibleContains(","))
            {
                var pair = typeString.SplitPair(',');

                typeString = pair.Key.Trim();
                assembly = pair.Value.Trim();
            }
            else
            {
                assembly = null;
            }

            var index = typeString.LastIndexOf('.');
            if (index > 0)
            {
                @namespace = typeString.Substring(0, index);
                name = typeString.Substring(index + 1);
            }
            else
            {
                @namespace = null;
                name = typeString;
            }

            return name.IsNotEmpty();
        }


        /// <summary>
        /// 尝试解析组合器。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeString"/> is null or empty.
        /// </exception>
        /// <param name="typeString">给定的类型字符串（支持仅名称、带命名空间、包含程序集等）。</param>
        /// <param name="result">输出 <see cref="TypeNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static bool TryParseCombiner(string typeString, out TypeNameCombiner result)
        {
            if (TryParseParameters(typeString, out var name, out var @namespace, out var assembly))
            {
                result = new TypeNameCombiner(name, @namespace, assembly);
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// 创建组合器。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public static TypeNameCombiner CreateCombiner<T>()
            => new TypeNameCombiner(typeof(T));

    }
}
