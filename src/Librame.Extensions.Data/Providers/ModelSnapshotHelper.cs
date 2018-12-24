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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 模型快照助手。
    /// </summary>
    public class ModelSnapshotHelper
    {

        /// <summary>
        /// 生成快照代码。
        /// </summary>
        /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <param name="snapshotName">给定的快照名称。</param>
        /// <returns>返回字符串。</returns>
        public static string GenerateSnapshotCode<TDbContext>(TDbContext dbContext, string snapshotName)
            where TDbContext : DbContext
        {
            var baseDbContextType = typeof(TDbContext);
            var implDbContextType = dbContext.GetType();
            var reporter = dbContext.GetService<IOperationReporter>();

            return new DesignTimeServicesBuilder(baseDbContextType.Assembly, implDbContextType.Assembly, reporter, new string[0])
                .Build(dbContext)
                .GetService<IMigrationsCodeGenerator>()
                .GenerateSnapshot(implDbContextType.Namespace, implDbContextType, snapshotName, dbContext.Model);
        }


        /// <summary>
        /// 创建快照。
        /// </summary>
        /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <param name="snapshotCode">给定的快照代码。</param>
        /// <param name="snapshotName">给定的快照名称。</param>
        /// <returns>返回 <see cref="ModelSnapshot"/>。</returns>
        public static ModelSnapshot CreateSnapshot<TDbContext>(TDbContext dbContext, string snapshotCode, string snapshotName)
            where TDbContext : DbContext
        {
            return CreateSnapshot(dbContext.GetType().Assembly, snapshotCode, snapshotName);
        }

        /// <summary>
        /// 创建快照。
        /// </summary>
        /// <param name="dbContextAssembly">给定的数据库上下文程序集。</param>
        /// <param name="snapshotCode">给定的快照代码。</param>
        /// <param name="snapshotName">给定的快照名称。</param>
        /// <returns>返回 <see cref="ModelSnapshot"/>。</returns>
        public static ModelSnapshot CreateSnapshot(Assembly dbContextAssembly, string snapshotCode, string snapshotName)
        {
            var dbContextAssemblyName = dbContextAssembly.GetName().Name;

            var references = dbContextAssembly
                .GetReferencedAssemblies()
                .Select(e => MetadataReference.CreateFromFile(Assembly.Load(e).Location))
                .Union(new MetadataReference[]
                {
                    MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
                    MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(dbContextAssembly.Location)
                });

            var compilation = CSharpCompilation.Create(dbContextAssemblyName)
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(snapshotCode));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if (!result.Success && result.Diagnostics.IsNotEmpty())
                    throw new ArgumentException(result.Diagnostics[0].ToString());

                var snapshot = Assembly.Load(ms.GetBuffer()).CreateInstance(dbContextAssemblyName + "." + snapshotName);
                return snapshot as ModelSnapshot;
            }
        }

    }
}
