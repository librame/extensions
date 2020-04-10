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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Librame.Extensions.Core.Compilers
{
    using Combiners;

    /// <summary>
    /// CSharp 编译器。
    /// </summary>
    public static class CSharpCompiler
    {
        /// <summary>
        /// 编译的文件扩展名。
        /// </summary>
        /// <remarks>
        /// 使用 .dll 扩展名会导致读写抛出未授权异常。
        /// </remarks>
        public const string FileExtension = ".dat";


        /// <summary>
        /// 在文件中编译模型快照。
        /// </summary>
        /// <param name="assemblyPath">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="references">给定的 <see cref="IEnumerable{MetadataReference}"/>。</param>
        /// <param name="sourceCodes">给定的源代码数组。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FilePathCombiner CompileInFile(FilePathCombiner assemblyPath, IEnumerable<MetadataReference> references,
            params string[] sourceCodes)
        {
            assemblyPath.NotNull(nameof(assemblyPath));

            var syntaxTrees = sourceCodes.Select(s => SyntaxFactory.ParseSyntaxTree(s));
            var compileOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemblyPath.FileName, syntaxTrees, references, compileOptions);

            using (var stream = File.OpenWrite(assemblyPath))
            {
                var result = compilation.Emit(stream);
                if (!result.Success)
                    throw new InvalidOperationException($"Build failed. Diagnostics: {string.Join(Environment.NewLine, result.Diagnostics)}");
            }

            return assemblyPath;
        }

        /// <summary>
        /// 在内存中编译模型快照。
        /// </summary>
        /// <param name="references">给定的 <see cref="IEnumerable{MetadataReference}"/>。</param>
        /// <param name="sourceCodes">给定的源代码数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] CompileInMemory(IEnumerable<MetadataReference> references, params string[] sourceCodes)
        {
            var assemlbyName = Path.GetRandomFileName();

            var syntaxTrees = sourceCodes.Select(s => SyntaxFactory.ParseSyntaxTree(s));
            var compileOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemlbyName, syntaxTrees, references, compileOptions);

            using (var stream = new MemoryStream())
            {
                var result = compilation.Emit(stream);
                if (!result.Success)
                    throw new InvalidOperationException($"Build failed. Diagnostics: {string.Join(Environment.NewLine, result.Diagnostics)}");

                return stream.ToArray();
            }
        }

    }
}
