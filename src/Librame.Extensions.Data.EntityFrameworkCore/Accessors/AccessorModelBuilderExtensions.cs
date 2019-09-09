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
    /// 访问器模型构建器静态扩展。
    /// </summary>
    public static class AccessorModelBuilderExtensions
    {
        /// <summary>
        /// 配置存储中心基类。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="DataBuilderOptions"/>。</param>
        public static void ConfigureStoreHubBase(this ModelBuilder modelBuilder,
            DataBuilderOptions options)
        {
            if (options.Tables.DefaultSchema.IsNotNullOrEmpty())
                modelBuilder.HasDefaultSchema(options.Tables.DefaultSchema);

            var mapRelationship = options.Stores?.MapRelationship ?? true;
            var maxLength = options.Stores?.MaxLengthForProperties ?? 0;

            // 审计
            modelBuilder.Entity<DataAudit>(b =>
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
                }

                if (mapRelationship)
                {
                    b.HasMany(p => p.Properties).WithOne(p => p.Audit)
                        .IsRequired().OnDelete(DeleteBehavior.Cascade);
                }
            });

            // 审计属性
            modelBuilder.Entity<DataAuditProperty>(b =>
            {
                b.ToTable(options.Tables.AuditPropertyFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.AuditId }).HasName();

                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.AuditId).HasMaxLength(256).IsRequired();

                if (maxLength > 0)
                {
                    b.Property(p => p.PropertyName).HasMaxLength(maxLength);
                    b.Property(p => p.PropertyTypeName).HasMaxLength(maxLength);
                }
            });

            // 实体
            modelBuilder.Entity<DataEntity>(b =>
            {
                b.ToTable(options.Tables.EntityFactory);

                b.HasKey(k => k.Id);

                b.HasIndex(i => new { i.Schema, i.Name }).HasName().IsUnique();

                b.Property(p => p.Id).HasMaxLength(256);
                b.Property(p => p.Schema).HasMaxLength(256).IsRequired();
                b.Property(p => p.Name).HasMaxLength(256).IsRequired();

                if (maxLength > 0)
                {
                    b.Property(p => p.EntityName).HasMaxLength(maxLength);
                    b.Property(p => p.AssemblyName).HasMaxLength(maxLength);
                    b.Property(p => p.Description).HasMaxLength(maxLength);
                    b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
                }
            });

            //// 模型
            //modelBuilder.Entity<DataMigration>(b =>
            //{
            //    b.ToTable(options.Tables.ModelFactory);

            //    b.HasKey(k => k.Id);

            //    b.HasIndex(i => new { i.AttributeId, i.ProductVersion }).HasName().IsUnique();

            //    b.Property(p => p.Id).HasMaxLength(256);
            //    b.Property(p => p.AttributeId).HasMaxLength(256).IsRequired();
            //    b.Property(p => p.ProductVersion).HasMaxLength(256).IsRequired();

            //    if (maxLength > 0)
            //    {
            //        b.Property(p => p.ModelHash).HasMaxLength(maxLength);
            //        b.Property(p => p.CreatedBy).HasMaxLength(maxLength);
            //    }
            //});

            // 租户
            modelBuilder.Entity<DataTenant>(b =>
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
                }
            });
        }

    }
}
