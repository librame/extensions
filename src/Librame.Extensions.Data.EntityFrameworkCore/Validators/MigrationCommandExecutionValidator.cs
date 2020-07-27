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
using System.Text;

namespace Librame.Extensions.Data.Validators
{
    using Core.Combiners;
    using Data.Accessors;
    using Data.Migrations;

    /// <summary>
    /// 迁移命令执行验证器。
    /// </summary>
    public class MigrationCommandExecutionValidator : IMigrationCommandExecutionValidator
    {
        private Encoding _defaultEncoding = null;


        /// <summary>
        /// 构造一个 <see cref="MigrationCommandExecutionValidator"/>。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        public MigrationCommandExecutionValidator(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache.NotNull(nameof(memoryCache));

            _defaultEncoding = ExtensionSettings.Preference.DefaultEncoding;
        }


        /// <summary>
        /// 内存缓存。
        /// </summary>
        public IMemoryCache MemoryCache { get; }


        /// <summary>
        /// 过滤已执行的迁移命令。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="commands">给定的 <see cref="IEnumerable{MigrationCommand}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationCommand}"/></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public IReadOnlyList<MigrationCommand> FilterExecuted(DbContextAccessorBase accessor,
            IEnumerable<MigrationCommand> commands)
        {
            accessor.NotNull(nameof(accessor));
            commands.NotNull(nameof(commands));

            var filePath = GetFilePathCombiner(accessor);
            var cacheInfos = MemoryCache.GetOrCreate(GetCacheKey(filePath), entry =>
            {
                if (filePath.Exists())
                    return filePath.ReadJson<List<MigrationCommandInfo>>(_defaultEncoding);

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
        /// 保存已执行的迁移命令。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationCommandInfo}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public IReadOnlyList<MigrationCommandInfo> SaveExecuted(DbContextAccessorBase accessor)
        {
            accessor.NotNull(nameof(accessor));

            var filePath = GetFilePathCombiner(accessor);

            var cacheInfos = MemoryCache.Get<List<MigrationCommandInfo>>(GetCacheKey(filePath));

            filePath.WriteJson(cacheInfos, _defaultEncoding);

            return cacheInfos;
        }


        private static FilePathCombiner GetFilePathCombiner(DbContextAccessorBase accessor)
            => DbContextAccessorHelper.GenerateAccessorFilePath(accessor, ".json",
                dependency => dependency.MigrationsConfigDirectory);

        private static string GetCacheKey(FilePathCombiner filePath)
            => DbContextAccessorHelper.GenerateAccessorKey(nameof(MigrationCommandExecutionValidator), filePath);

    }
}
