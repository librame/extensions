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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 内部访问器模型构建器静态扩展。
    /// </summary>
    internal static class InternalAccessorModelBuilderExtensions
    {
        /// <summary>
        /// 配置基础实体集合。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        public static void ConfigureBaseEntities(this ModelBuilder modelBuilder, DataBuilderOptions options)
        {
            if (!options.DefaultSchema.IsNullOrEmpty())
                modelBuilder.HasDefaultSchema(options.DefaultSchema);

            // 审计
            modelBuilder.Entity<Audit>(b =>
            {
                b.ToTable(options.TableSchemas.AuditFactory);

                b.HasKey(k => k.Id);

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.EntityName).HasMaxLength(100).IsRequired();
                b.Property(p => p.EntityTypeName).HasMaxLength(200).IsRequired();
                b.Property(p => p.StateName).HasMaxLength(50);
                b.Property(p => p.CreatedBy).HasMaxLength(50);

                // 关联
                b.HasMany(p => p.Properties).WithOne(p => p.Audit).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            // 审计属性
            modelBuilder.Entity<AuditProperty>(b =>
            {
                b.ToTable(options.TableSchemas.AuditPropertyFactory);

                b.HasKey(k => k.Id);

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.PropertyName).HasMaxLength(100).IsRequired();
                b.Property(p => p.PropertyTypeName).HasMaxLength(200).IsRequired();
            });

            // 租户
            modelBuilder.Entity<Tenant>(b =>
            {
                b.ToTable(options.TableSchemas.TenantFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.Name, i.Host }).HasName().IsUnique();

                b.Property(p => p.Id).ValueGeneratedNever();
                b.Property(p => p.Name).HasMaxLength(100).IsRequired();
                b.Property(p => p.Host).HasMaxLength(200).IsRequired();
            });
        }

    }
}
