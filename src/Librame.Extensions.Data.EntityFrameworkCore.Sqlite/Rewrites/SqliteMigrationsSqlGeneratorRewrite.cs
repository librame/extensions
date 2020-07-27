#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore.Migrations
{
    /// <summary>
    /// <see cref="SqliteMigrationsSqlGenerator"/> 改写。
    /// </summary>
    public class SqliteMigrationsSqlGeneratorRewrite : SqliteMigrationsSqlGenerator
    {
        /// <summary>
        /// 构造一个 <see cref="SqliteMigrationsSqlGeneratorRewrite"/>。
        /// </summary>
        /// <param name="dependencies">给定的 <see cref="MigrationsSqlGeneratorDependencies"/>。</param>
        /// <param name="migrationsAnnotations">给定的 <see cref="IMigrationsAnnotationProvider"/>。</param>
        public SqliteMigrationsSqlGeneratorRewrite(MigrationsSqlGeneratorDependencies dependencies,
            IMigrationsAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations)
        {
        }


        #region Invalid migration operations

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        /// <param name="terminate"> Indicates whether or not to terminate the command after generating SQL for the operation. </param>
        protected override void Generate(AddForeignKeyOperation operation, IModel model,
            MigrationCommandListBuilder builder, bool terminate = true)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        /// <param name="terminate"> Indicates whether or not to terminate the command after generating SQL for the operation. </param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected override void Generate(AddPrimaryKeyOperation operation, IModel model,
            MigrationCommandListBuilder builder, bool terminate = true)
        {
            // 创建分表操作已包含添加主键操作，所以如遇分表添加主键操作时可直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        protected override void Generate(AddUniqueConstraintOperation operation, IModel model,
            MigrationCommandListBuilder builder)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        protected override void Generate(CreateCheckConstraintOperation operation, IModel model,
            MigrationCommandListBuilder builder)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        /// <param name="terminate"> Indicates whether or not to terminate the command after generating SQL for the operation. </param>
        protected override void Generate(DropColumnOperation operation, IModel model,
            MigrationCommandListBuilder builder, bool terminate = true)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        /// <param name="terminate"> Indicates whether or not to terminate the command after generating SQL for the operation. </param>
        protected override void Generate(DropForeignKeyOperation operation, IModel model,
            MigrationCommandListBuilder builder, bool terminate = true)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        /// <param name="terminate"> Indicates whether or not to terminate the command after generating SQL for the operation. </param>
        protected override void Generate(DropPrimaryKeyOperation operation, IModel model,
            MigrationCommandListBuilder builder, bool terminate = true)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        protected override void Generate(DropUniqueConstraintOperation operation, IModel model,
            MigrationCommandListBuilder builder)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        protected override void Generate(DropCheckConstraintOperation operation, IModel model,
            MigrationCommandListBuilder builder)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Throws <see cref="NotSupportedException" /> since this operation requires table rebuilds, which
        ///     are not yet supported.
        /// </summary>
        /// <param name="operation"> The operation. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to build the commands. </param>
        protected override void Generate(AlterColumnOperation operation, IModel model,
            MigrationCommandListBuilder builder)
        {
            // 默认直接过滤
            //throw new NotSupportedException(
            //    SqliteStrings.InvalidMigrationOperation(operation.GetType().ShortDisplayName()));
        }

        /// <summary>
        ///     Generates a SQL fragment for a computed column definition for the given column metadata.
        /// </summary>
        /// <param name="schema"> The schema that contains the table, or <c>null</c> to use the default schema. </param>
        /// <param name="table"> The table that contains the column. </param>
        /// <param name="name"> The column name. </param>
        /// <param name="operation"> The column metadata. </param>
        /// <param name="model"> The target model which may be <c>null</c> if the operations exist without a model. </param>
        /// <param name="builder"> The command builder to use to add the SQL fragment. </param>
        protected override void ComputedColumnDefinition(string schema, string table, string name,
            ColumnOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            // 默认直接过滤
            //throw new NotSupportedException(SqliteStrings.ComputedColumnsNotSupported);
        }

        #endregion

    }
}
