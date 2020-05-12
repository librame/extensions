#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Migrations
{
    using Core.Builders;
    using Core.Combiners;
    using Data.Accessors;
    using Data.Builders;
    using Data.Compilers;

    /// <summary>
    /// 迁移命令过滤器。
    /// </summary>
    public static class MigrationCommandFiltrator
    {
        private static FilePathCombiner GetFilePathCombiner(IAccessor accessor)
            => ModelSnapshotCompiler.CombineFilePath(accessor, d => d.MigrationsDirectory, ".json");

        private static string GetCacheKey(FilePathCombiner filePath)
            => $"{nameof(MigrationCommandFiltrator)}:{filePath}";


        /// <summary>
        /// 过滤命令。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="commands">给定的 <see cref="IEnumerable{MigrationCommand}"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="CoreBuilderOptions"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationCommand}"/></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IReadOnlyList<MigrationCommand> Filter(IMemoryCache memoryCache,
            IAccessor accessor, IEnumerable<MigrationCommand> commands,
            DataBuilderOptions options, CoreBuilderOptions coreOptions)
        {
            memoryCache.NotNull(nameof(memoryCache));
            commands.NotNull(nameof(commands));
            options.NotNull(nameof(options));
            coreOptions.NotNull(nameof(coreOptions));

            var filePath = GetFilePathCombiner(accessor);
            var cacheInfos = memoryCache.GetOrCreate(GetCacheKey(filePath), entry =>
            {
                if (filePath.Exists())
                    return filePath.ReadJson<List<MigrationCommandInfo>>();

                return new List<MigrationCommandInfo>();
            });

            var execCommands = new List<MigrationCommand>();
            foreach (var command in commands)
            {
                var info = ToInfo(command);
                if (!cacheInfos.Contains(info))
                {
                    cacheInfos.Add(info);
                    execCommands.Add(command);
                }
            }

            return execCommands;

            // ToInfo
            MigrationCommandInfo ToInfo(MigrationCommand command)
            {
                var tenant = accessor.CurrentTenant;
                return new MigrationCommandInfo
                {
                    Text = command.CommandText,
                    // 为防止连接字符串信息泄露，此处仅用连接代称
                    ConnectionString = $"Name={tenant?.Name};Host={tenant?.Host};ConnectionStringTag={accessor.GetCurrentConnectionStringTag()}"
                };
            }
        }


        /// <summary>
        /// 保存过滤命令。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="options">给定的 <see cref="CoreBuilderOptions"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void Save(IMemoryCache memoryCache, IAccessor accessor,
            CoreBuilderOptions options)
        {
            memoryCache.NotNull(nameof(memoryCache));
            options.NotNull(nameof(options));

            var filePath = GetFilePathCombiner(accessor);
            var cacheInfos = memoryCache.Get<List<MigrationCommandInfo>>(GetCacheKey(filePath));
            filePath.WriteJson(cacheInfos, options.Encoding);
        }

    }
}
