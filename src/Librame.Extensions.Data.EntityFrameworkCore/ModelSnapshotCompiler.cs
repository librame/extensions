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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Librame.Extensions.Data
{
    using Core;
    using Resources;

    /// <summary>
    /// 模型快照编译器。
    /// </summary>
    public static class ModelSnapshotCompiler
    {
        // 使用 .dll 扩展名会导致读写抛出未授权异常
        private const string ExportAssemblyFileExtension = ".dat";


        /// <summary>
        /// 生成模型快照名称。
        /// </summary>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public static TypeNameCombiner GenerateTypeName(Type accessorType)
            => new TypeNameCombiner(accessorType?.Namespace, $"{accessorType.GetBodyName()}{nameof(ModelSnapshot)}");

        /// <summary>
        /// 获取模型快照程序集路径。
        /// </summary>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner GetAssemblyPath(Type accessorType, string basePath)
            => new FilePathCombiner($"{accessorType?.Assembly.GetSimpleName()}.ModelSnapshot{ExportAssemblyFileExtension}").ChangeBasePathIfEmpty(basePath);


        /// <summary>
        /// 还原程序集。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] RestoreAssembly(byte[] buffer)
            => buffer?.RtlDecompress();

        /// <summary>
        /// 存储程序集。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] StoreAssembly(byte[] buffer)
            => buffer?.RtlCompress();


        /// <summary>
        /// 获取程序集引用列表。
        /// </summary>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回包含 <see cref="AssemblyReference"/> 的 <see cref="IReadOnlyList{T}"/>。</returns>
        public static IReadOnlyList<AssemblyReference> GetAssemblyReferences(DataBuilderOptions options, Type accessorType)
        {
            //var baseReferences = new List<AssemblyReference>
            //{
            //    AssemblyReference.ByName("Microsoft.EntityFrameworkCore"),
            //    AssemblyReference.ByName("Microsoft.EntityFrameworkCore.Relational"),
            //    AssemblyReference.ByName("Librame.Extensions.Data.Abstractions"),
            //    AssemblyReference.ByName("Librame.Extensions.Data.EntityFrameworkCore"),
            //    // Add DesignTimeServices AssemblyReference,
            //    // Add DbContextAccessor AssemblyReference,
            //    AssemblyReference.ByName("netstandard"),
            //    AssemblyReference.ByName("System.Runtime")
            //};

            var references = new List<AssemblyReference>(options?.MigrationAssemblyReferences);

            // Add DbContextAccessor AssemblyReference
            var dbContextAccessorReference = AssemblyReference.ByAssembly(accessorType?.Assembly);
            if (!references.Contains(dbContextAccessorReference))
                references.Add(dbContextAccessorReference);

            references.Sort();

            return references.AsReadOnlyList();
        }


        #region Compile

        /// <summary>
        /// 编译模型快照。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="model">给定的 <see cref="IModel"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <returns>返回包含字节数组与哈希的元组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FilePathCombiner CompileInFile(IAccessor accessor, IModel model, DataBuilderOptions options)
        {
            accessor.NotNull(nameof(accessor));
            model.NotNull(nameof(model));

            var accessorType = accessor.GetType();
            var typeName = GenerateTypeName(accessorType);

            var generator = accessor.ServiceFactory.GetRequiredService<IMigrationsCodeGenerator>();
            var dependencyOptions = accessor.ServiceFactory.GetRequiredService<DataBuilderDependencyOptions>();

            var sourceCode = generator.GenerateSnapshot(typeName.Namespace, accessorType,
                typeName.Name, model);

            // 导出包含模型快照的程序集文件
            var exportAssemblyPath = GetAssemblyPath(accessorType, dependencyOptions.BaseDirectory);
            var references = GetAssemblyReferences(options, accessorType);

            return CompileInFile(exportAssemblyPath, references, sourceCode);
        }

        /// <summary>
        /// 在文件中编译模型快照。
        /// </summary>
        /// <param name="assemblyPath">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="references">给定的 <see cref="IEnumerable{AssemblyReference}"/>。</param>
        /// <param name="sourceCodes">给定的源代码数组。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FilePathCombiner CompileInFile(FilePathCombiner assemblyPath, IEnumerable<AssemblyReference> references,
            params string[] sourceCodes)
        {
            assemblyPath.NotNull(nameof(assemblyPath));

            if (!assemblyPath.FileName.IsExtension(ExportAssemblyFileExtension))
                throw new InvalidOperationException(InternalResource.InvalidOperationExceptionCompileFileExtension);

            var metadatas = new List<MetadataReference>();
            foreach (var reference in references)
            {
                if (reference.CopyLocal)
                {
                    if (reference.Location.IsEmpty())
                        throw new InvalidOperationException(InternalResource.InvalidOperationExceptionNotFindPathForAssemblyReferenceFormat.Format(reference.ToString()));

                    File.Copy(reference.Location, Path.Combine(assemblyPath.BasePath, Path.GetFileName(reference.Location)), overwrite: true);
                }
                metadatas.AddRange(reference.Metadatas);
            }

            var syntaxTrees = sourceCodes.Select(s => SyntaxFactory.ParseSyntaxTree(s));
            var compileOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemblyPath.FileName, syntaxTrees, metadatas, compileOptions);

            using (var stream = File.OpenWrite(assemblyPath))
            {
                var result = compilation.Emit(stream);
                if (!result.Success)
                    throw new InvalidOperationException($"Build failed. Diagnostics: {string.Join(Environment.NewLine, result.Diagnostics)}");
            }

            return assemblyPath;
        }


        /// <summary>
        /// 编译模型快照。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="model">给定的 <see cref="IModel"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <param name="typeName">给定的模型快照 <see cref="TypeNameCombiner"/>。</param>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回包含字节数组与哈希的元组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static (byte[] Body, string Hash) CompileInMemory(IAccessor accessor, IModel model,
            DataBuilderOptions options, TypeNameCombiner typeName, Type accessorType)
        {
            accessor.NotNull(nameof(accessor));
            model.NotNull(nameof(model));
            typeName.NotNull(nameof(typeName));
            accessorType.NotNull(nameof(accessorType));

            var generator = accessor.ServiceFactory.GetRequiredService<IMigrationsCodeGenerator>();

            var sourceCode = generator.GenerateSnapshot(typeName.Namespace, accessorType,
                typeName.Name, model);

            var references = GetAssemblyReferences(options, accessorType);

            var buffer = CompileInMemory(references, sourceCode);
            return (StoreAssembly(buffer), sourceCode.Sha256Base64String());
        }

        /// <summary>
        /// 在内存中编译模型快照。
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

        #endregion

    }
}
