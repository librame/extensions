#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    using Core.Utilities;

    /// <summary>
    /// 程序集描述符。
    /// </summary>
    public class AssemblyDescriptor : IEquatable<AssemblyDescriptor>,
        IEqualityComparer<AssemblyDescriptor>, IComparable<AssemblyDescriptor>, IComparable
    {
        /// <summary>
        /// 构造一个 <see cref="AssemblyDescriptor"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="compiled">给定的已编译程序集。</param>
        /// <param name="isItself">是否为已编译程序集自身（可选）。</param>
        /// <param name="library">给定的 <see cref="CompilationLibrary"/>（可选）。</param>
        public AssemblyDescriptor(string name, Assembly compiled,
            bool? isItself = null, CompilationLibrary library = null)
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
        [SuppressMessage("Design", "CA1031:不捕获常规异常类型", Justification = "<挂起>")]
        public virtual MetadataReference ToMetadataReference()
        {
            try
            {
                return MetadataReference.CreateFromFile(Compiled.Location);
            }
            catch (Exception)
            {
                try
                {
                    return MetadataReference.CreateFromImage(Compiled.SerializeBinary());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="x">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="y">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回的布尔值。</returns>
        public virtual bool Equals(AssemblyDescriptor x, AssemblyDescriptor y)
            => x?.Equals(y) == true;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(AssemblyDescriptor other)
            => Name == other?.Name && Compiled == other.Compiled;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回的布尔值。</returns>
        public override bool Equals(object obj)
            => obj is AssemblyDescriptor other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => GetHashCode(this);

        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="obj">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int GetHashCode(AssemblyDescriptor obj)
            => obj.IsNull() ? -1
            : obj.Name.CompatibleGetHashCode() ^ obj.Compiled.GetHashCode();


        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="other">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(AssemblyDescriptor other)
            => string.CompareOrdinal(Name, other?.Name);

        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(object obj)
            => obj is AssemblyDescriptor other ? CompareTo(other) : -1;


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Name;


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AssemblyDescriptor left, AssemblyDescriptor right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);
            
            return left.Equals(right);
        }

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AssemblyDescriptor left, AssemblyDescriptor right)
            => !(left == right);

        /// <summary>
        /// 比较小于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(AssemblyDescriptor left, AssemblyDescriptor right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 比较小于等于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(AssemblyDescriptor left, AssemblyDescriptor right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 比较大于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(AssemblyDescriptor left, AssemblyDescriptor right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 比较大于等于。
        /// </summary>
        /// <param name="left">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="AssemblyDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(AssemblyDescriptor left, AssemblyDescriptor right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;


        /// <summary>
        /// 创建指定程序集的描述符。
        /// </summary>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <returns>返回 <see cref="AssemblyDescriptor"/>。</returns>
        public static AssemblyDescriptor Create(Assembly assembly)
        {
            assembly.NotNull(nameof(assembly));

            var assemblyName = assembly.GetDisplayName();
            var library = GetLibrary(assemblyName, out var isItself);

            return new AssemblyDescriptor(assemblyName, assembly, isItself, library);
        }

        /// <summary>
        /// 创建指定程序集名称的描述符（支持优先从当前程序集列表中创建）。
        /// </summary>
        /// <param name="assemblyName">给定的程序集名称。</param>
        /// <returns>返回 <see cref="AssemblyDescriptor"/>。</returns>
        public static AssemblyDescriptor Create(string assemblyName)
        {
            assemblyName.NotEmpty(nameof(assemblyName));

            var compiled = AssemblyUtility.CurrentAssemblies
                .FirstOrDefault(p => p.GetDisplayName() == assemblyName);

            if (compiled.IsNotNull())
            {
                var library = GetLibrary(assemblyName, out var isItself);
                return new AssemblyDescriptor(assemblyName, compiled, isItself, library);
            }

            try
            {
                compiled = Assembly.Load(new AssemblyName(assemblyName));
                var library = GetLibrary(assemblyName, out var isItself);

                return new AssemblyDescriptor(assemblyName, compiled, isItself, library);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 从当前程序集列表中创建描述符。
        /// </summary>
        /// <param name="predicate">给定要导入程序集的断定方法。</param>
        /// <returns>返回 <see cref="AssemblyDescriptor"/>。</returns>
        [SuppressMessage("Globalization", "CA1303:请不要将文本作为本地化参数传递", Justification = "<挂起>")]
        public static AssemblyDescriptor CreateFromCurrentAssemblies(Func<Assembly, bool> predicate)
        {
            predicate.NotNull(nameof(predicate));

            var compiled = AssemblyUtility.CurrentAssemblies.FirstOrDefault(predicate);
            if (compiled.IsNull())
                throw new InvalidOperationException("No assembly found.");

            var assemblyName = compiled.GetDisplayName();
            var library = GetLibrary(assemblyName, out var isItself);

            return new AssemblyDescriptor(assemblyName, compiled, isItself, library);
        }

        private static CompilationLibrary GetLibrary(string assemblyName, out bool isItself)
        {
            foreach (var library in DependencyContext.Default.CompileLibraries)
            {
                if (library.Name == assemblyName)
                {
                    isItself = true;
                    return library;
                }

                if (library.Assemblies.Contains(assemblyName))
                {
                    isItself = false;
                    return library;
                }
            }

            isItself = false;
            return null;
        }

    }
}
