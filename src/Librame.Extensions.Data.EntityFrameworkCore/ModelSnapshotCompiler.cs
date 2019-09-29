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
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 模型快照编译器。
    /// </summary>
    public class ModelSnapshotCompiler
    {
        /// <summary>
        /// 在内存中编译。
        /// </summary>
        /// <param name="references">给定的 <see cref="IEnumerable{AssemblyReference}"/>。</param>
        /// <param name="sourceCodes">给定的源代码数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] CompileInMemory(IEnumerable<AssemblyReference> references, params string[] sourceCodes)
        {
            var assemlbyName = Path.GetRandomFileName();

            var metadatas = references.SelectMany(descr => descr.Metadatas);
            
            var syntaxTrees = sourceCodes.Select(s => SyntaxFactory.ParseSyntaxTree(s));
            var compileOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemlbyName, syntaxTrees, metadatas, compileOptions);

            using (var stream = new MemoryStream())
            {
                var result = compilation.Emit(stream);
                if (!result.Success)
                    throw new InvalidOperationException($"Build failed. Diagnostics: {string.Join(Environment.NewLine, result.Diagnostics)}");

                return stream.ToArray();
            }
        }

        /// <summary>
        /// 在文件中编译。
        /// </summary>
        /// <param name="filePath">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="references">给定的 <see cref="IEnumerable{AssemblyReference}"/>。</param>
        /// <param name="sourceCodes">给定的源代码数组。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner CompileInFile(FilePathCombiner filePath, IEnumerable<AssemblyReference> references,
            params string[] sourceCodes)
        {
            filePath.NotNull(nameof(filePath));

            if (!filePath.FileName.IsExtension(".dll"))
                throw new InvalidOperationException("Only supported “.dll” file extension.");

            var metadatas = new List<MetadataReference>();
            foreach (var reference in references)
            {
                if (reference.CopyLocal)
                {
                    if (string.IsNullOrEmpty(reference.FilePath))
                        throw new InvalidOperationException("Could not find path for reference " + reference);

                    File.Copy(reference.FilePath, Path.Combine(filePath.BasePath, Path.GetFileName(reference.FilePath)), overwrite: true);
                }
                metadatas.AddRange(reference.Metadatas);
            }

            var syntaxTrees = sourceCodes.Select(s => SyntaxFactory.ParseSyntaxTree(s));
            var compileOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(filePath.FileName, syntaxTrees, metadatas, compileOptions);

            using (var stream = File.OpenWrite(filePath))
            {
                var result = compilation.Emit(stream);
                if (!result.Success)
                    throw new InvalidOperationException($"Build failed. Diagnostics: {string.Join(Environment.NewLine, result.Diagnostics)}");
            }

            return filePath;
        }

    }
}
