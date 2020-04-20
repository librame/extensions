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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Data.Compilers
{
    using Core;
    using Core.Compilers;
    using Core.Builders;
    using Core.Combiners;
    using Data.Accessors;
    using Data.Builders;

    /// <summary>
    /// 模型快照编译器。
    /// </summary>
    public static class ModelSnapshotCompiler
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyList<MetadataReference>> _references
            = new ConcurrentDictionary<Type, IReadOnlyList<MetadataReference>>();


        /// <summary>
        /// 生成模型快照名称。
        /// </summary>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <returns>返回 <see cref="TypeNameCombiner"/>。</returns>
        public static TypeNameCombiner GenerateTypeName(Type accessorType)
            => new TypeNameCombiner(accessorType?.Namespace, $"{accessorType.GetGenericBodyName()}{nameof(ModelSnapshot)}");


        /// <summary>
        /// 导出模型快照文件路径。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="basePathFactory">给定基础路径的工厂方法（可选；默认为模型快照目录）。</param>
        /// <param name="extension">给定的扩展名（可选；默认为 <see cref="CSharpCompiler.FileExtension"/>）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "accessor")]
        public static FilePathCombiner CombineFilePath(IAccessor accessor,
            Func<DataBuilderDependency, string> basePathFactory = null,
            string extension = CSharpCompiler.FileExtension)
        {
            accessor.NotNull(nameof(accessor));
            
            var dependency = accessor.GetService<DataBuilderDependency>();
            var basePath = basePathFactory?.Invoke(dependency) ?? dependency.ModelSnapshotsDirectory;

            var builder = accessor.GetService<IDataBuilder>();
            return CombineFilePath(accessor.CurrentType, builder.DatabaseDesignTimeType,
                basePath, accessor.IsWritingConnectionString(), extension);
        }

        /// <summary>
        /// 导出模型快照文件路径。
        /// </summary>
        /// <param name="accessorType">给定的访问器类型。</param>
        /// <param name="designTimeType">给定的设计时类型。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="isWritingConnectionString">是写入连接字符串。</param>
        /// <param name="extension">给定的扩展名（可选；默认为 <see cref="CSharpCompiler.FileExtension"/>）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner CombineFilePath(Type accessorType, Type designTimeType,
            string basePath, bool isWritingConnectionString, string extension = CSharpCompiler.FileExtension)
        {
            var fileName = isWritingConnectionString ? "WritingConnection" : "DefaultConnection";
            fileName = $"{accessorType.GetDisplayName(true)}.{designTimeType.GetDisplayName(true)}.{fileName}{extension}";

            var filePath = new FilePathCombiner(fileName, basePath);
            filePath.CreateDirectory();

            return filePath;
        }


        /// <summary>
        /// 还原程序集。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        internal static byte[] RestoreAssembly(byte[] buffer)
            => buffer?.RtlDecompress();

        /// <summary>
        /// 存储程序集。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        internal static byte[] StoreAssembly(byte[] buffer)
            => buffer?.RtlCompress();


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

            var generator = accessor.GetService<IMigrationsCodeGenerator>();
            var sourceCode = generator.GenerateSnapshot(typeName.Namespace, accessorType,
                typeName.Name, model);

            // 导出包含模型快照的程序集文件
            var exportAssemblyPath = CombineFilePath(accessor);
            var references = GetMetadataReferences(options, accessorType);

            return CSharpCompiler.CompileInFile(exportAssemblyPath, references, sourceCode);
        }

        /// <summary>
        /// 编译模型快照。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="model">给定的 <see cref="IModel"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <param name="typeName">给定的模型快照 <see cref="TypeNameCombiner"/>。</param>
        /// <returns>返回包含字节数组与哈希的元组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static (byte[] Body, string Hash) CompileInMemory(IAccessor accessor, IModel model,
            DataBuilderOptions options, TypeNameCombiner typeName)
        {
            accessor.NotNull(nameof(accessor));
            model.NotNull(nameof(model));
            typeName.NotNull(nameof(typeName));

            var generator = accessor.GetService<IMigrationsCodeGenerator>();
            var coreOptions = accessor.GetService<IOptions<CoreBuilderOptions>>().Value;

            var sourceCode = generator.GenerateSnapshot(typeName.Namespace, accessor.CurrentType,
                typeName.Name, model);

            var references = GetMetadataReferences(options, accessor.CurrentType);

            var buffer = CSharpCompiler.CompileInMemory(references, sourceCode);
            return (StoreAssembly(buffer), sourceCode.Sha256Base64String(coreOptions.Encoding));
        }


        private static IReadOnlyList<MetadataReference> GetMetadataReferences(DataBuilderOptions options, Type accessorType)
        {
            return _references.GetOrAdd(accessorType, t =>
            {
                var references = new List<AssemblyReference>(options?.MigrationAssemblyReferences.Distinct());

                // Add DbContextAccessor AssemblyReference
                var accessorReference = AssemblyReference.Load(accessorType?.Assembly);
                if (!references.Contains(accessorReference))
                    references.Add(accessorReference);

                references.Sort();

                return references.Select(refer => refer.ToMetadataReference()).AsReadOnlyList();
            });

            //var metadatas = new List<MetadataReference>();
            //foreach (var reference in references)
            //{
            //    if (reference.CopyLocal)
            //    {
            //        if (reference.Location.IsEmpty())
            //            throw new InvalidOperationException(InternalResource.InvalidOperationExceptionNotFindPathForAssemblyReferenceFormat.Format(reference.ToString()));

            //        File.Copy(reference.Location, Path.Combine(assemblyPath.BasePath, Path.GetFileName(reference.Location)), overwrite: true);
            //    }
            //    metadatas.AddRange(reference.Metadatas);
            //}
        }

    }
}
