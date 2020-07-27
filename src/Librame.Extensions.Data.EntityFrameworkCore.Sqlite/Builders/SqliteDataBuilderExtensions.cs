#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Sqlite.Design.Internal;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// SQLite 数据构建器静态扩展。
    /// </summary>
    public static class SqliteDataBuilderExtensions
    {
        /// <summary>
        /// 添加 SQLite 数据库服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static IDataBuilder AddSqliteServices(this IDataBuilder builder)
        {
            builder.AddDatabaseDesignTime<SqliteDesignTimeServices>();

            return builder;
        }


        /// <summary>
        /// 替换 SQLite 访问器服务集合。
        /// </summary>
        /// <param name="optionsBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
        /// <returns>返回 <see cref="DbContextOptionsBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static DbContextOptionsBuilder ReplaceSqliteAccessorServices(this DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.NotNull(nameof(optionsBuilder));

            //optionsBuilder.ReplaceService<IMigrationsSqlGenerator, SqliteMigrationsSqlGeneratorRewrite>();

            return optionsBuilder;
        }

    }
}
