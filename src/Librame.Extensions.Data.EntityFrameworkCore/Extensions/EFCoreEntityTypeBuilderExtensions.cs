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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="EntityTypeBuilder"/> 静态扩展。
    /// </summary>
    public static class EFCoreEntityTypeBuilderExtensions
    {
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

    }
}
