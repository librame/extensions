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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 程序集引用。
    /// </summary>
    public class AssemblyReference : IEquatable<AssemblyReference>, IEqualityComparer<AssemblyReference>
    {
        private AssemblyReference(IEnumerable<MetadataReference> metadatas,
            bool copyLocal = false, string path = null)
        {
            Metadatas = metadatas.NotEmpty(nameof(metadatas));
            CopyLocal = copyLocal;
            FilePath = path;
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
        /// 文件路径。
        /// </summary>
        public string FilePath { get; }


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
        /// <param name="x">给定的 <see cref="AssemblyReference"/>。</param>
        /// <param name="y">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回的布尔值。</returns>
        public bool Equals(AssemblyReference x, AssemblyReference y)
            => x?.Equals(y) == true;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="obj">给定的 <see cref="AssemblyReference"/>。</param>
        /// <returns>返回整数。</returns>
        public int GetHashCode(AssemblyReference obj)
        {
            if (obj.IsNull())
                return -1;

            return obj.Metadatas.ComputeHashCode();
        }


        /// <summary>
        /// 通过程序集引用。
        /// </summary>
        /// <param name="assembly">给定的程序集。</param>
        /// <param name="copyLocal">是否复制到本地（可选；默认不复制）。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        public static AssemblyReference ByAssembly(Assembly assembly, bool copyLocal = false)
        {
            var metadata = (MetadataReference)MetadataReference.CreateFromFile(assembly.Location);
            return new AssemblyReference(metadata.YieldEnumerable(), copyLocal);
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

            var metadatas = DependencyContext.Default.CompileLibraries
                .Where(lib => assemblyName == lib.Name)
                .Select(lib =>
                {
                    var assembly = Assembly.LoadFrom($"{assemblyName}.dll");
                    return (MetadataReference)MetadataReference.CreateFromFile(assembly.Location);
                })
                .ToArray();

            if (metadatas.IsEmpty())
                throw new InvalidOperationException($"Assembly '{assemblyName}' not found.");
            
            return new AssemblyReference(metadatas, copyLocal);
        }

        /// <summary>
        /// 通过程序集路径引用。
        /// </summary>
        /// <param name="assemblyPath">给定的程序集路径。</param>
        /// <returns>返回 <see cref="AssemblyReference"/>。</returns>
        public static AssemblyReference ByPath(string assemblyPath)
        {
            var metadatas = MetadataReference.CreateFromFile(assemblyPath).YieldEnumerable();
            return new AssemblyReference(metadatas, path: assemblyPath);
        }
    }
}
