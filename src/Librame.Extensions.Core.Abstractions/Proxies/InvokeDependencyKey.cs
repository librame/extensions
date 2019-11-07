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
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 调用依赖键名。
    /// </summary>
    public class InvokeDependencyKey : IEquatable<InvokeDependencyKey>
    {
        /// <summary>
        /// 构造一个 <see cref="InvokeDependencyKey"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        /// <param name="kind">给定的依赖种类。</param>
        public InvokeDependencyKey(string name, InvokeDependencyKind kind)
        {
            Name = name.NotEmpty(nameof(name));
            Kind = kind;
        }


        /// <summary>
        /// 依赖名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 依赖种类。
        /// </summary>
        public InvokeDependencyKind Kind { get; }


        /// <summary>
        /// 获取指定依赖名称、种类的调用名称集合。
        /// </summary>
        /// <returns>返回字符串集合。</returns>
        public IEnumerable<string> GetInvokeNames()
        {
            switch (Kind)
            {
                case InvokeDependencyKind.Property:
                    {
                        InvokeDependencyKindHelper.TryGetDefaultValue(nameof(InvokeDependencyKind.Property), out string defaultValue);
                        var pair = defaultValue.SplitPair(',');
                        return new string[] { pair.Key + Name, pair.Value + Name };
                    }

                case InvokeDependencyKind.PropertyGet:
                    {
                        InvokeDependencyKindHelper.TryGetDefaultValue(nameof(InvokeDependencyKind.PropertyGet), out string defaultValue);
                        return (defaultValue + Name).YieldEnumerable();
                    }

                case InvokeDependencyKind.PropertySet:
                    {
                        InvokeDependencyKindHelper.TryGetDefaultValue(nameof(InvokeDependencyKind.PropertySet), out string defaultValue);
                        return (defaultValue + Name).YieldEnumerable();
                    }

                default:
                    return Name.YieldEnumerable();
            }
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(InvokeDependencyKey other)
            => Name == other?.Name && Kind == other.Kind;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is InvokeDependencyKey other) ? Equals(other) : false;


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{nameof(Name)}={Name},{nameof(Kind)}={Kind.ToString()}";


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="b">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(InvokeDependencyKey a, InvokeDependencyKey b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <param name="b">给定的 <see cref="InvokeDependencyKey"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(InvokeDependencyKey a, InvokeDependencyKey b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="key">给定的 <see cref="InvokeDependencyKey"/>。</param>
        public static implicit operator string(InvokeDependencyKey key)
            => key?.ToString();


        /// <summary>
        /// 解析与方法信息近似的键名集合。
        /// </summary>
        /// <param name="methodInfo">给定的 <see cref="MethodInfo"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{T}"/> 的 <see cref="InvokeDependencyKey"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "methodInfo")]
        public static IReadOnlyList<InvokeDependencyKey> ParseApproximateKeys(MethodInfo methodInfo)
        {
            methodInfo.NotNull(nameof(methodInfo));

            var list = new List<InvokeDependencyKey>();

            if (methodInfo.Name.CompatibleContains("_"))
            {
                var pair = methodInfo.Name.SplitPair('_');
                switch (pair.Key)
                {
                    case "get":
                        {
                            list.Add(new InvokeDependencyKey(pair.Value, InvokeDependencyKind.PropertyGet));
                            list.Add(new InvokeDependencyKey(pair.Value, InvokeDependencyKind.Property));
                            break;
                        }

                    case "set":
                        {
                            list.Add(new InvokeDependencyKey(pair.Value, InvokeDependencyKind.PropertySet));
                            list.Add(new InvokeDependencyKey(pair.Value, InvokeDependencyKind.Property));
                            break;
                        }

                    default:
                        break;
                }
            }

            if (list.IsEmpty())
                list.Add(new InvokeDependencyKey(methodInfo.Name, InvokeDependencyKind.Property));

            return list.AsReadOnlyList();
        }

    }
}
