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
    /// 抽象表架构静态扩展。
    /// </summary>
    public static class AbstractionTableSchemaExtensions
    {
        /// <summary>
        /// 应用架构。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="schema"/> 为 NULL 或空字符串。
        /// </exception>
        /// <param name="tableSchema">给定的 <see cref="ITableSchema"/>。</param>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema ApplySchema(this ITableSchema tableSchema, string schema)
        {
            if (tableSchema.IsNotNull())
                tableSchema.Schema = schema.NotNullOrEmpty(nameof(schema));

            return tableSchema;
        }

    }
}
