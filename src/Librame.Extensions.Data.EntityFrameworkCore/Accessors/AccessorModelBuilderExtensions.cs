#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Accessors
{
    using Protectors;
    using Stores;

    /// <summary>
    /// 访问器模型构建器静态扩展。
    /// </summary>
    public static class AccessorModelBuilderExtensions
    {
        /// <summary>
        /// 配置数据存储集合。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void ConfigureDataStores<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
            (this ModelBuilder modelBuilder, DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> accessor)
            where TAudit : DataAudit<TGenId, TCreatedBy>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TMigration : DataMigration<TGenId, TCreatedBy>
            where TTabulation : DataTabulation<TGenId, TCreatedBy>
            where TTenant : DataTenant<TGenId, TCreatedBy>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
            where TCreatedBy : IEquatable<TCreatedBy>
            => modelBuilder.ConfigureDataStores<TAudit, TAuditProperty, TMigration, TTabulation, TTenant,
                TGenId, TIncremId, TCreatedBy>(accessor as DbContextAccessorBase);

        /// <summary>
        /// 配置数据存储集合。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="accessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void ConfigureDataStores<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
            (this ModelBuilder modelBuilder, DbContextAccessorBase accessor)
            where TAudit : DataAudit<TGenId, TCreatedBy>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TMigration : DataMigration<TGenId, TCreatedBy>
            where TTabulation : DataTabulation<TGenId, TCreatedBy>
            where TTenant : DataTenant<TGenId, TCreatedBy>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
            where TCreatedBy : IEquatable<TCreatedBy>
        {
            modelBuilder.NotNull(nameof(modelBuilder));
            accessor.NotNull(nameof(accessor));

            var maxLength = accessor.Dependency.Options.Stores.MaxLengthForProperties;
            var protectPrivacyData = accessor.Dependency.Options.Stores.ProtectPrivacyData;
            var tables = accessor.Dependency.Options.Tables;

            var isGenIdString = typeof(TGenId).IsStringType();

            if (tables.DefaultSchema.IsNotEmpty())
                modelBuilder.HasDefaultSchema(tables.DefaultSchema);
            
            // 审计
            modelBuilder.Entity<TAudit>(b =>
            {
                b.ToTable(t =>
                {
                    if (tables.UseDataPrefix)
                        t.InsertDataPrefix();

                    t.Configure(tables.Audit);
                });

                b.HasIndex(i => new { i.TableName, i.EntityId, i.State }).HasName().IsUnique();

                b.HasKey(k => k.Id);

                b.Property(p => p.Id).ValueGeneratedNever();

                if (maxLength > 0)
                {
                    if (isGenIdString)
                        b.Property(p => p.Id).HasMaxLength(maxLength);

                    b.Property(p => p.EntityId).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.TableName).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.StateName).HasMaxLength(maxLength);

                    // 在 MySQL 中如果长度超出 255 会被转换为不能作为主键或唯一性约束 的 BLOB/TEXT 类型
                    b.Property(p => p.EntityTypeName).HasMaxLength(maxLength);
                }
            });

            // 审计属性
            modelBuilder.Entity<TAuditProperty>(b =>
            {
                b.ToTable(t =>
                {
                    if (tables.UseDataPrefix)
                        t.InsertDataPrefix();

                    // 按年月分表
                    t.AppendYearAndMonthSuffix(accessor.CurrentTimestamp)
                        .Configure(tables.AuditProperty);
                });

                b.HasIndex(i => new { i.AuditId }).HasName();

                b.HasKey(k => k.Id);

                b.Property(x => x.Id).ValueGeneratedOnAdd();

                if (maxLength > 0)
                {
                    b.Property(p => p.AuditId).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.PropertyName).HasMaxLength(maxLength);
                    b.Property(p => p.PropertyTypeName).HasMaxLength(maxLength);
                }

                // MaxLength
                b.Property(p => p.OldValue);
                b.Property(p => p.NewValue);
            });

            // 迁移
            modelBuilder.Entity<TMigration>(b =>
            {
                b.ToTable(t =>
                {
                    if (tables.UseDataPrefix)
                        t.InsertDataPrefix();

                    t.Configure(tables.Migration);
                });

                b.HasIndex(i => i.ModelHash).HasName().IsUnique();

                b.HasKey(k => k.Id);

                b.Property(p => p.Id).ValueGeneratedNever();

                if (maxLength > 0)
                {
                    if (isGenIdString)
                        b.Property(p => p.Id).HasMaxLength(maxLength);

                    b.Property(p => p.ModelHash).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.AccessorName).HasMaxLength(maxLength);
                    b.Property(p => p.ModelSnapshotName).HasMaxLength(maxLength);
                }
            });

            // 表格
            modelBuilder.Entity<TTabulation>(b =>
            {
                b.ToTable(t =>
                {
                    if (tables.UseDataPrefix)
                        t.InsertDataPrefix();

                    t.Configure(tables.Tabulation);
                });

                b.HasIndex(i => new { i.Schema, i.TableName }).HasName().IsUnique();

                b.HasKey(k => k.Id);

                b.Property(p => p.Id).ValueGeneratedNever();

                if (maxLength > 0)
                {
                    if (isGenIdString)
                        b.Property(p => p.Id).HasMaxLength(maxLength);

                    b.Property(p => p.Schema).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.TableName).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.EntityName).HasMaxLength(maxLength);
                    b.Property(p => p.AssemblyName).HasMaxLength(maxLength);
                    b.Property(p => p.Description).HasMaxLength(maxLength);
                }

                b.Property(p => p.IsSharding).HasDefaultValue(false);
            });

            // 租户
            modelBuilder.Entity<TTenant>(b =>
            {
                b.ToTable(t =>
                {
                    if (tables.UseDataPrefix)
                        t.InsertDataPrefix();

                    t.Configure(tables.Tenant);
                });

                b.HasIndex(i => new { i.Name, i.Host }).HasName().IsUnique();

                b.HasKey(k => k.Id);

                b.Property(p => p.Id).ValueGeneratedNever();

                if (maxLength > 0)
                {
                    if (isGenIdString)
                        b.Property(p => p.Id).HasMaxLength(maxLength);

                    b.Property(p => p.Name).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.Host).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.DefaultConnectionString).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.WritingConnectionString).HasMaxLength(maxLength);
                }

                if (protectPrivacyData)
                {
                    var protector = accessor.ApplicationServiceProvider.GetService<IPrivacyDataProtector>();
                    b.ConfigurePrivacyData(protector);
                }
            });
        }

    }
}
