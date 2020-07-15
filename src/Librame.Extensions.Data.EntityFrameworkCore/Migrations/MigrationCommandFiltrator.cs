#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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

    /// <summary>
    /// 迁移命令过滤器。
    /// </summary>
    public static class MigrationCommandFiltrator
    {
        private static FilePathCombiner GetFilePathCombiner(DbContextAccessorBase accessor)
            => DbContextAccessorHelper.GenerateAccessorFilePath(accessor, ".json",
                dependency => dependency.MigrationsConfigDirectory);

        private static string GetCacheKey(FilePathCombiner filePath)
            => DbContextAccessorHelper.GenerateAccessorKey(nameof(MigrationCommandFiltrator), filePath);


        /// <summary>
        /// 过滤命令。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="commands">给定的 <see cref="IEnumerable{MigrationCommand}"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="CoreBuilderOptions"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationCommand}"/></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IReadOnlyList<MigrationCommand> Filter(IMemoryCache memoryCache,
            DbContextAccessorBase accessor, IEnumerable<MigrationCommand> commands,
            CoreBuilderOptions coreOptions)
        {
            memoryCache.NotNull(nameof(memoryCache));
            commands.NotNull(nameof(commands));
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
                var info = new MigrationCommandInfo(command.CommandText)
                    .SetConnectionString(accessor);

                if (!cacheInfos.Contains(info))
                {
                    cacheInfos.Add(info);
                    execCommands.Add(command);
                }
            }

            return execCommands;
        }


        /// <summary>
        /// 保存过滤命令。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="options">给定的 <see cref="CoreBuilderOptions"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void Save(IMemoryCache memoryCache, DbContextAccessorBase accessor,
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
