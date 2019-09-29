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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Text;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 索引构建器静态扩展。
    /// </summary>
    public static class EFCoreIndexBuilderExtensions
    {
        /// <summary>
        /// 有指定名称的索引（格式：EntityName+PropertyNames+Index）。
        /// </summary>
        /// <param name="indexBuilder">给定的 <see cref="IndexBuilder"/>。</param>
        /// <param name="nameFactory">给定的名称工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IndexBuilder"/>。</returns>
        public static IndexBuilder HasName(this IndexBuilder indexBuilder, Func<string, string> nameFactory = null)
        {
            var sb = new StringBuilder();

            // Prefix: EntityBodyName
            sb.Append(indexBuilder.Metadata.DeclaringEntityType.ClrType.GetBodyName());

            foreach (var property in indexBuilder.Metadata.Properties)
                sb.Append(property.Name);

            // Suffix: Index
            sb.Append("Index");

            var name = sb.ToString();

            return indexBuilder.HasName(nameFactory.IsNotNull() ? nameFactory.Invoke(name) : name);
        }

    }
}
