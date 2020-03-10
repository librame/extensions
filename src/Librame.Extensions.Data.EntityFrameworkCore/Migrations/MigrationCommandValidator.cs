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
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Migrations
{
    using Core.Builders;
    using Core.Combiners;
    using Core.Services;
    using Data.Accessors;
    using Data.Builders;

    /// <summary>
    /// 迁移命令验证器。
    /// </summary>
    public static class MigrationCommandValidator
    {
        private static List<MigrationCommandInfo> _cacheInfos
            = new List<MigrationCommandInfo>();


        /// <summary>
        /// 验证命令。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="commands">给定的 <see cref="IEnumerable{MigrationCommand}"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="CoreBuilderOptions"/>。</param>
        /// <returns>返回 <see cref="IEnumerable{MigrationCommand}"/></returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IEnumerable<MigrationCommand> Validate(IAccessor accessor, IEnumerable<MigrationCommand> commands,
            DataBuilderOptions options, CoreBuilderOptions coreOptions)
        {
            accessor.NotNull(nameof(accessor));
            commands.NotNull(nameof(commands));
            options.NotNull(nameof(options));
            coreOptions.NotNull(nameof(coreOptions));

            var dependency = accessor.ServiceFactory.GetService<DataBuilderDependency>();
            var filePath = new FilePathCombiner($"{accessor.GetType().GetAssemblyDisplayName()}.ValidateMigrationCommands.txt")
                .ChangeBasePathIfEmpty(dependency.ExportDirectory);
            if (filePath.Exists())
            {
                var json = filePath.ReadAllText(coreOptions.Encoding.Source);
                _cacheInfos = JsonConvert.DeserializeObject<List<MigrationCommandInfo>>(json);
            }

            var isChanged = false;
            foreach (var command in commands)
            {
                if (command.IsNull())
                    continue;

                var info = GenerateInfo(accessor, command);

                if (!_cacheInfos.Contains(info))
                {
                    if (!isChanged)
                        isChanged = true;

                    _cacheInfos.Add(info);
                    yield return command;
                }
            }

            if (options.ExportMigrationCommands)
            {
                var json = JsonConvert.SerializeObject(_cacheInfos);
                filePath.WriteAllText(json, coreOptions.Encoding.Source);
            }
        }


        private static MigrationCommandInfo GenerateInfo(IAccessor accessor, MigrationCommand command)
            => new MigrationCommandInfo
            {
                Text = command.CommandText,
                ConnectionString = accessor.CurrentConnectionString
            };

    }
}
