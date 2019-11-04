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
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 程序集引用。
    /// </summary>
    public class AssemblyReference : IEquatable<AssemblyReference>, IEqualityComparer<AssemblyReference>, IComparable<AssemblyReference>, IComparable
    {
        private AssemblyReference(IEnumerable<MetadataReference> metadatas,
            bool copyLocal = false, string location = null)
        {
            Metadatas = metadatas.NotEmpty(nameof(metadatas));
            CopyLocal = copyLocal;
            Location = location;
        }


        /// <summary>
        /// 元数据集合。
        /// </summary>
        public IEnumerable<MetadataReference> Metadatas { get; }

        /// <summary>
        /// 复制到本地。
        /// </summary>
        public bool CopyLocal { get; }

        /// <summary>
        /// 已加载文件的完整路径或 UNC 位置。
        /// </summary>
        public string Location { get; }


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
        {
            if (other.IsNull() || other.Metadatas.IsEmpty()
                || Metadatas.Count() != other.Metadatas.Count())
            {
                return false;
            }

            for (var i = 0; i < Metadatas.Count(); i++)
            {
                if (Metadatas.ElementAt(i).Display != other.Metadatas.ElementAt(i).Display)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回的布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is AssemblyReference other) ? Equals(other) : false;


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
        {
            if (obj.IsNull()) return -1;
            return obj.Metadatas.ComputeHashCode();
        }


        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="other">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回整数。</returns>
        public int CompareTo(AssemblyReference other)
            => string.Compare(Location, other?.Location, StringComparison.OrdinalIgnoreCase);

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
            => string.Join(',', Metadatas.Select(s => s.Display));


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
        /// 通过程序集引用。
        /// </summary>
        /// <param name="assembly">给定的程序集。</param>
        /// <param name="copyLocal">是否复制到本地（可选；默认不复制）。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "assembly")]
        public static AssemblyReference ByAssembly(Assembly assembly, bool copyLocal = false)
        {
            MetadataReference metadata = MetadataReference.CreateFromFile(assembly.Location);
            return new AssemblyReference(metadata.YieldEnumerable(), copyLocal, assembly.Location);
        }

        /// <summary>
        /// 通过程序集名称引用。
        /// </summary>
        /// <param name="assemblyName">给定的程序集名称。</param>
        /// <param name="copyLocal">是否复制到本地（可选；默认不复制）。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        public static AssemblyReference ByName(string assemblyName, bool copyLocal = false)
        {
            assemblyName.NotEmpty(nameof(assemblyName));

            var assembly = AssemblyUtility.CurrentDomainAssemblies
                .FirstOrDefault(assembly => assemblyName.Equals(assembly.GetSimpleName(), StringComparison.OrdinalIgnoreCase));

            if (assembly.IsNull())
                throw new InvalidOperationException($"Assembly '{assemblyName}' not found.");

            MetadataReference reference = MetadataReference.CreateFromFile(assembly.Location);
            return new AssemblyReference(reference.YieldEnumerable(), copyLocal, assembly.Location);
        }

        /// <summary>
        /// 通过程序集路径引用。
        /// </summary>
        /// <param name="assemblyPath">给定的程序集路径。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        public static AssemblyReference ByPath(string assemblyPath)
        {
            var metadatas = MetadataReference.CreateFromFile(assemblyPath).YieldEnumerable();
            return new AssemblyReference(metadatas, location: assemblyPath);
        }
    }
}
