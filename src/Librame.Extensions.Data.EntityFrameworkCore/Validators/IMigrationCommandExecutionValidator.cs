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

namespace Librame.Extensions.Data.Validators
{
    using Accessors;
    using Migrations;

    /// <summary>
    /// 迁移命令执行验证器接口。
    /// </summary>
    public interface IMigrationCommandExecutionValidator
    {
        /// <summary>
        /// 内存缓存。
        /// </summary>
        IMemoryCache MemoryCache { get; }


        /// <summary>
        /// 过滤已执行的迁移命令。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="commands">给定的 <see cref="IEnumerable{MigrationCommand}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationCommand}"/></returns>
        IReadOnlyList<MigrationCommand> FilterExecuted(DbContextAccessorBase accessor,
            IEnumerable<MigrationCommand> commands);

        /// <summary>
        /// 保存已执行的迁移命令。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationCommandInfo}"/>。</returns>
        IReadOnlyList<MigrationCommandInfo> SaveExecuted(DbContextAccessorBase accessor);
    }
}
