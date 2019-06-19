#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 表架构静态扩展。
    /// </summary>
    public static class TableSchemaExtensions
    {
        /// <summary>
        /// 当作内部表架构（格式参考：__names?）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="formatter">给定的表名格式化器（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsInternalTableSchema(this Type entityType,
            Func<string, string> formatter = null, string schema = null)
        {
            return entityType.AsTableSchema(names =>
            {
                var format = $"__{names}";

                if (formatter.IsNotNull())
                    format = formatter.Invoke(format);

                return format;
            },
            schema);
        }

        /// <summary>
        /// 当作表架构（格式参考：names?）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="formatter">给定的表名格式化器（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsTableSchema(this Type entityType,
            Func<string, string> formatter = null, string schema = null)
        {
            var names = TableSchema.GetEntityTypeNames(entityType);

            if (formatter.IsNotNull())
                names = formatter.Invoke(names);

            return new TableSchema(names, schema);
        }

    }
}
