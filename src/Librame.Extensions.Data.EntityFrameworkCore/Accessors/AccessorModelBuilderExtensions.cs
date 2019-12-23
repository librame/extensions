#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Accessors
{
    using Builders;
    using Schemas;
    using Stores;

    /// <summary>
    /// 访问器模型构建器静态扩展。
    /// </summary>
    public static class AccessorModelBuilderExtensions
    {
        /// <summary>
        /// 配置数据存储中心。
        /// </summary>
        /// <typeparam name="TAudit">指定的审计类型。</typeparam>
        /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
        /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void ConfigureDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
            (this ModelBuilder modelBuilder, DataBuilderOptions options)
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
        {
            modelBuilder.NotNull(nameof(modelBuilder));
            options.NotNull(nameof(options));

            if (options.Tables.DefaultSchema.IsNotEmpty())
                modelBuilder.HasDefaultSchema(options.Tables.DefaultSchema);

            var mapRelationship = options.Stores?.MapRelationship ?? true;
            var maxLength = options.Stores?.MaxLengthForProperties ?? 0;

            // 审计
            modelBuilder.Entity<TAudit>(b =>
            {
                b.ToTable(options.Tables.AuditFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.EntityTypeName, i.State }).HasName();

                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.EntityTypeName).HasMaxLength(256).IsRequired();
                
                if (maxLength > 0)
                {
                    b.Property(p => p.TableName).HasMaxLength(maxLength);
                    b.Property(p => p.EntityId).HasMaxLength(maxLength);
                    b.Property(p => p.StateName).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedTimeTicks).HasMaxLength(maxLength);
                }
            });

            // 审计属性
            modelBuilder.Entity<TAuditProperty>(b =>
            {
                // 按年份季度分表
                b.ToTable(descr => descr.ChangeDateOffsetSuffixByYearQuarter(),
                    options.Tables.AuditPropertyFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.AuditId }).HasName();

                b.Property(p => p.Id).HasMaxLength(256).ValueGeneratedOnAdd();
                b.Property(p => p.AuditId).HasMaxLength(256).IsRequired();

                if (maxLength > 0)
                {
                    b.Property(p => p.PropertyName).HasMaxLength(maxLength);
                    b.Property(p => p.PropertyTypeName).HasMaxLength(maxLength);
                }
            });

            // 实体
            modelBuilder.Entity<TEntity>(b =>
            {
                b.ToTable(options.Tables.EntityFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.Schema, i.Name }).HasName().IsUnique();

                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.Schema).HasMaxLength(256).IsRequired();
                b.Property(p => p.Name).HasMaxLength(256).IsRequired();
                b.Property(p => p.IsSharding).HasDefaultValue(false);

                if (maxLength > 0)
                {
                    b.Property(p => p.EntityName).HasMaxLength(maxLength);
                    b.Property(p => p.AssemblyName).HasMaxLength(maxLength);
                    b.Property(p => p.Description).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedTimeTicks).HasMaxLength(maxLength);
                }
            });

            // 迁移
            modelBuilder.Entity<TMigration>(b =>
            {
                b.ToTable(options.Tables.MigrationFactory);

                b.HasKey(k => k.Id);

                // 不做唯一索引
                b.HasIndex(i => i.ModelHash).HasName();

                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.ModelHash).HasMaxLength(256).IsRequired();

                if (maxLength > 0)
                {
                    b.Property(p => p.AccessorName).HasMaxLength(maxLength);
                    b.Property(p => p.ModelSnapshotName).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedTimeTicks).HasMaxLength(maxLength);
                }
            });

            // 租户
            modelBuilder.Entity<TTenant>(b =>
            {
                b.ToTable(options.Tables.TenantFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.Name, i.Host }).HasName().IsUnique();

                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.Name).HasMaxLength(256).IsRequired();
                b.Property(p => p.Host).HasMaxLength(256).IsRequired();

                if (maxLength > 0)
                {
                    b.Property(p => p.DefaultConnectionString).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.WritingConnectionString).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedTimeTicks).HasMaxLength(maxLength);
                    b.Property(p => p.UpdatedBy).HasMaxLength(maxLength);
                    b.Property(p => p.UpdatedTimeTicks).HasMaxLength(maxLength);
                }
            });
        }

    }
}
