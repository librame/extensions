﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
using System.Reflection;

namespace Librame.Extensions.Core.Compilers
{
    using Combiners;

    /// <summary>
    /// CSharp 编译器。
    /// </summary>
    public static class CSharpCompiler
    {
        /// <summary>
        /// 从文件中反编译为程序集。
        /// </summary>
        /// <param name="assemblyPath">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="Assembly"/>。</returns>
        public static Assembly DecompileFromFile(FilePathCombiner assemblyPath)
        {
            var buffer = File.ReadAllBytes(assemblyPath);
            return Assembly.Load(buffer);
        }


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
