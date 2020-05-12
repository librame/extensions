#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data;
using Librame.Extensions.Data.Protectors;
using Librame.Extensions.Data.ValueConverters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="EntityTypeBuilder"/> 静态扩展。
    /// </summary>
    public static class EFCoreEntityTypeBuilderExtensions
    {

        #region ConfigurePropertyConversions

        /// <summary>
        /// 配置已标记隐私数据特性的属性转换集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        /// <param name="protector">给定的 <see cref="IPrivacyDataProtector"/>。</param>
        public static void ConfigurePrivacyData(this EntityTypeBuilder builder, IPrivacyDataProtector protector)
            => builder.ConfigurePrivacyData(new PrivacyDataConverter(protector));

        /// <summary>
        /// 配置已标记隐私数据特性的属性转换集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        /// <param name="converter">给定的 <see cref="PrivacyDataConverter"/>。</param>
        internal static void ConfigurePrivacyData(this EntityTypeBuilder builder, PrivacyDataConverter converter)
            => builder.ConfigurePropertyConversion<PrivacyDataAttribute>(converter);


        /// <summary>
        /// 配置已标记特性的属性转换集合。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        /// <param name="converter">给定的 <see cref="ValueConverter"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static void ConfigurePropertyConversion<TAttribute>(this EntityTypeBuilder builder, ValueConverter converter)
            where TAttribute : Attribute
        {
            builder.NotNull(nameof(builder));
            converter.NotNull(nameof(converter));

            var attributeType = typeof(TAttribute);
            var attributeProperties = builder.Metadata.ClrType.GetProperties()
                .Where(p => Attribute.IsDefined(p, attributeType));

            foreach (var property in attributeProperties)
            {
                if (property.PropertyType != converter.ModelClrType)
                    throw new InvalidOperationException($"[{nameof(TAttribute).TrimEnd(nameof(Attribute))}] only works {converter.ModelClrType} by default.");

                builder.Property(property.PropertyType, property.Name).HasConversion(converter);
            }
        }

        #endregion


        #region ToTable

        /// <summary>
        /// 映射表格。
        /// </summary>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder"/>。</param>
        /// <param name="action">给定的表描述符配置动作（可选）。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static EntityTypeBuilder ToTable(this EntityTypeBuilder builder, Action<TableDescriptor> action = null)
        {
            builder.NotNull(nameof(builder));

            var table = TableDescriptor.Create(builder.Metadata.ClrType);
            action?.Invoke(table);

            if (table.Schema.IsNotEmpty())
                return builder.ToTable(table, table.Schema);

            return builder.ToTable(table);
        }

        #endregion

    }
}
