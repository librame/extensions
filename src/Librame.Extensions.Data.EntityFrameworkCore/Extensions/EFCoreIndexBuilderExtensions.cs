﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="IndexBuilder"/> 静态扩展。
    /// </summary>
    public static class EFCoreIndexBuilderExtensions
    {
        /// <summary>
        /// 有指定名称的索引（格式：EntityName+PropertyNames+Index）。
        /// </summary>
        /// <param name="indexBuilder">给定的 <see cref="IndexBuilder"/>。</param>
        /// <param name="nameFactory">给定的名称工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IndexBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IndexBuilder HasName(this IndexBuilder indexBuilder, Func<string, string> nameFactory = null)
        {
            indexBuilder.NotNull(nameof(indexBuilder));

            var sb = new StringBuilder();

            // Prefix: EntityBodyName
            sb.Append(indexBuilder.Metadata.DeclaringEntityType.ClrType.GetGenericBodyName());

            foreach (var property in indexBuilder.Metadata.Properties)
                sb.Append(property.Name);

            // Suffix: Index
            sb.Append("Index");

            var name = sb.ToString();
            return indexBuilder.HasName(nameFactory?.Invoke(name) ?? name);
        }

    }
}
