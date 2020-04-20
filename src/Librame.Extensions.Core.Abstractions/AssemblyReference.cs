#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    using Core.Utilities;

    /// <summary>
    /// 程序集引用。
    /// </summary>
    public class AssemblyReference : IEquatable<AssemblyReference>, IEqualityComparer<AssemblyReference>, IComparable<AssemblyReference>, IComparable
    {
        /// <summary>
        /// 构造一个 <see cref="AssemblyReference"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="compiled">给定的已编译程序集。</param>
        /// <param name="isItself">是否为已编译程序集自身（可选）。</param>
        /// <param name="library">给定的 <see cref="CompilationLibrary"/>（可选）。</param>
        public AssemblyReference(string name, Assembly compiled, bool? isItself = null, CompilationLibrary library = null)
        {
            Name = name.NotEmpty(nameof(name));
            Compiled = compiled.NotNull(nameof(compiled));

            IsItself = isItself ?? Name == compiled.GetDisplayName();
            Library = library;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 已编译程序集。
        /// </summary>
        public Assembly Compiled { get; }

        /// <summary>
        /// 是否为已编译程序集自身。
        /// </summary>
        public bool IsItself { get; }

        /// <summary>
        /// 编译库。
        /// </summary>
        public CompilationLibrary Library { get; }


        /// <summary>
        /// 转换为元数据引用。
        /// </summary>
        /// <returns>返回 <see cref="MetadataReference"/>。</returns>
        public MetadataReference ToMetadataReference()
            => MetadataReference.CreateFromFile(Compiled.Location);


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="x">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="y">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回的布尔值。</returns>
        public bool Equals(AssemblyReference x, AssemblyReference y)
            => x?.Equals(y) == true;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(AssemblyReference other)
            => Name == other?.Name && Compiled == other.Compiled;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回的布尔值。</returns>
        public override bool Equals(object obj)
            => obj is AssemblyReference other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => GetHashCode(this);

        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="obj">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回整数。</returns>
        public int GetHashCode(AssemblyReference obj)
            => obj.IsNull() ? -1
            : obj.Name.CompatibleGetHashCode() ^ obj.Compiled.GetHashCode();


        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="other">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回整数。</returns>
        public int CompareTo(AssemblyReference other)
            => string.CompareOrdinal(Name, other?.Name);

        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回整数。</returns>
        public int CompareTo(object obj)
            => obj is AssemblyReference other ? CompareTo(other) : -1;


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Name;


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AssemblyReference left, AssemblyReference right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AssemblyReference left, AssemblyReference right)
            => !(left == right);

        /// <summary>
        /// 比较小于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(AssemblyReference left, AssemblyReference right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 比较小于等于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(AssemblyReference left, AssemblyReference right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 比较大于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(AssemblyReference left, AssemblyReference right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 比较大于等于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(AssemblyReference left, AssemblyReference right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;


        /// <summary>
        /// 加载程序集引用。
        /// </summary>
        /// <param name="assembly">给定的程序集。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        public static AssemblyReference Load(Assembly assembly)
            => new AssemblyReference(assembly.GetDisplayName(), assembly, true);

        /// <summary>
        /// 加载程序集引用。
        /// </summary>
        /// <param name="assemblyName">给定的程序集名称。</param>
        /// <param name="throwIfError">如果加载失败则抛出异常（可选；默认抛出异常）。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        public static AssemblyReference Load(string assemblyName, bool throwIfError = true)
        {
            var compiled = AssemblyUtility.CurrentAssemblies.SingleOrDefault(p => p.GetDisplayName() == assemblyName);
            if (compiled.IsNotNull())
                return new AssemblyReference(assemblyName, compiled, true);

            foreach (var library in DependencyContext.Default.CompileLibraries)
            {
                if (library.Name == assemblyName)
                {
                    compiled = AppDomain.CurrentDomain.Load(assemblyName);
                    return new AssemblyReference(assemblyName, compiled, true, library);
                }

                if (library.Assemblies.Contains(assemblyName))
                {
                    compiled = AppDomain.CurrentDomain.Load(assemblyName);
                    return new AssemblyReference(assemblyName, compiled, false, library);
                }
            }
            
            if (throwIfError)
                throw new ArgumentException($"Load assembly '{assemblyName}' error.");

            return null;
        }

    }
}
