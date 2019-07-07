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

        #region AsInternalTableSchema

        /// <summary>
        /// 当作表架构（格式参考：__names_suffix）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffixFactory">给定的 <see cref="DateTimeOffset"/> 后缀工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsInternalTableSchema(this Type entityType,
            Func<DateTimeOffset, string> suffixFactory, string schema = null)
        {
            suffixFactory.NotNull(nameof(suffixFactory));

            var suffix = suffixFactory.Invoke(DateTimeOffset.Now);

            Func<string, string> namesFactory = names => $"{names}_{suffix}";

            return entityType.AsInternalTableSchema(namesFactory, schema);
        }

        /// <summary>
        /// 当作内部表架构（格式参考：__names?）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="namesFactory">给定的表名工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsInternalTableSchema(this Type entityType,
            Func<string, string> namesFactory = null, string schema = null)
        {
            Func<string, string> _namesFactory = names =>
            {
                var format = $"__{names}";

                if (namesFactory.IsNotNull())
                    format = namesFactory.Invoke(format);

                return format;
            };

            return entityType.AsTableSchema(_namesFactory, schema);
        }

        #endregion


        #region AsTableSchema

        /// <summary>
        /// 当作表架构（格式参考：names_suffix）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffixFactory">给定的 <see cref="DateTimeOffset"/> 后缀工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsTableSchema(this Type entityType,
            Func<DateTimeOffset, string> suffixFactory, string schema = null)
        {
            suffixFactory.NotNull(nameof(suffixFactory));

            var suffix = suffixFactory.Invoke(DateTimeOffset.Now);

            Func<string, string> namesFactory = names => $"{names}_{suffix}";

            return entityType.AsTableSchema(namesFactory, schema);
        }

        /// <summary>
        /// 当作表架构（格式参考：names?）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="namesFactory">给定的表名工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsTableSchema(this Type entityType,
            Func<string, string> namesFactory = null, string schema = null)
        {
            var names = TableSchema.GetEntityNames(entityType);

            if (namesFactory.IsNotNull())
                names = namesFactory.Invoke(names);

            return new TableSchema(names, schema);
        }

        #endregion

    }
}
