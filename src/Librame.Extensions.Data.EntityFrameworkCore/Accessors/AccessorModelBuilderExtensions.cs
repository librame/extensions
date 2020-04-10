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
        /// <param name="accessor">给定的数据库上下文访问器。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void ConfigureDataStoreHub<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
            (this ModelBuilder modelBuilder, DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> accessor)
            where TAudit : DataAudit<TGenId>
            where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
            where TEntity : DataEntity<TGenId>
            where TMigration : DataMigration<TGenId>
            where TTenant : DataTenant<TGenId>
            where TGenId : IEquatable<TGenId>
            where TIncremId : IEquatable<TIncremId>
        {
            modelBuilder.NotNull(nameof(modelBuilder));
            accessor.NotNull(nameof(accessor));

            if (accessor.BuilderOptions.Tables.DefaultSchema.IsNotEmpty())
                modelBuilder.HasDefaultSchema(accessor.BuilderOptions.Tables.DefaultSchema);

            var maxLength = accessor.BuilderOptions.Stores.MaxLengthForProperties;

            // 审计
            modelBuilder.Entity<TAudit>(b =>
            {
                b.ToTable(table =>
                {
                    table.Configure(accessor.BuilderOptions.Tables.Audit);
                });
                
                b.HasKey(k => k.Id);
                
                b.HasIndex(i => new { i.EntityTypeName, i.State }).HasName();

                if (maxLength > 0)
                {
                    b.Property(p => p.Id).HasMaxLength(maxLength);
                    b.Property(p => p.EntityId).HasMaxLength(maxLength);
                    b.Property(p => p.TableName).HasMaxLength(maxLength);
                    b.Property(p => p.StateName).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);

                    // 在 MySQL 中如果长度超出 255 会被转换为不能作为主键或唯一性约束 的 BLOB/TEXT 类型
                    b.Property(p => p.EntityTypeName).HasMaxLength(maxLength).IsRequired();
                }
            });

            // 审计属性
            modelBuilder.Entity<TAuditProperty>(b =>
            {
                b.ToTable(table =>
                {
                    // 按年月分表
                    table.AppendYearAndMonthSuffix(accessor.CurrentTimestamp)
                        .Configure(accessor.BuilderOptions.Tables.AuditProperty);
                });

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.AuditId }).HasName();

                b.Property(p => p.Id).ValueGeneratedOnAdd();

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

            // 实体
            modelBuilder.Entity<TEntity>(b =>
            {
                b.ToTable(table =>
                {
                    table.Configure(accessor.BuilderOptions.Tables.Entity);
                });

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.Schema, i.Name }).HasName().IsUnique();

                if (maxLength > 0)
                {
                    b.Property(p => p.Id).HasMaxLength(maxLength);
                    b.Property(p => p.Schema).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.Name).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.EntityName).HasMaxLength(maxLength);
                    b.Property(p => p.AssemblyName).HasMaxLength(maxLength);
                    b.Property(p => p.Description).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                }

                b.Property(p => p.IsSharding).HasDefaultValue(false);
            });

            // 迁移
            modelBuilder.Entity<TMigration>(b =>
            {
                b.ToTable(table =>
                {
                    table.Configure(accessor.BuilderOptions.Tables.Migration);
                });

                b.HasKey(k => k.Id);

                // 不做唯一索引
                b.HasIndex(i => i.ModelHash).HasName();

                if (maxLength > 0)
                {
                    b.Property(p => p.Id).HasMaxLength(maxLength);
                    b.Property(p => p.ModelHash).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.AccessorName).HasMaxLength(maxLength);
                    b.Property(p => p.ModelSnapshotName).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                }
            });

            // 租户
            modelBuilder.Entity<TTenant>(b =>
            {
                b.ToTable(table =>
                {
                    table.Configure(accessor.BuilderOptions.Tables.Tenant);
                });

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.Name, i.Host }).HasName().IsUnique();

                if (maxLength > 0)
                {
                    b.Property(p => p.Id).HasMaxLength(maxLength);
                    b.Property(p => p.Name).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.Host).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.DefaultConnectionString).HasMaxLength(maxLength).IsRequired();
                    b.Property(p => p.WritingConnectionString).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                    b.Property(p => p.UpdatedBy).HasMaxLength(maxLength);
                }
            });
        }

    }
}
