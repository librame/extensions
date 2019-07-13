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
        /// 当作内部当前日期与时间表架构（格式参考：__names_suffix）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffixFactory">给定的 <see cref="DateTimeOffset"/> 后缀工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsInternalDateTimeTableSchema(this Type entityType,
            Func<DateTime, string> suffixFactory, string schema = null)
        {
            suffixFactory.NotNull(nameof(suffixFactory));

            var suffix = suffixFactory.Invoke(DateTime.Now);

            return entityType.AsInternalSuffixTableSchema(suffix, schema);
        }

        /// <summary>
        /// 当作内部当前日期与时间偏移表架构（格式参考：__names_suffix）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffixFactory">给定的 <see cref="DateTimeOffset"/> 后缀工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsInternalDateTimeOffsetTableSchema(this Type entityType,
            Func<DateTimeOffset, string> suffixFactory, string schema = null)
        {
            suffixFactory.NotNull(nameof(suffixFactory));

            var suffix = suffixFactory.Invoke(DateTimeOffset.Now);

            return entityType.AsInternalSuffixTableSchema(suffix, schema);
        }

        /// <summary>
        /// 当作内部后缀表架构（格式参考：__names_?）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffix">给定的表名后缀。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsInternalSuffixTableSchema(this Type entityType,
            string suffix, string schema = null)
        {
            return entityType.AsInternalTableSchema(names => $"{names}_{suffix}", schema);
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
        /// 当作当前日期与时间表架构（格式参考：names_suffix）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffixFactory">给定的 <see cref="DateTimeOffset"/> 后缀工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsDateTimeTableSchema(this Type entityType,
            Func<DateTime, string> suffixFactory, string schema = null)
        {
            suffixFactory.NotNull(nameof(suffixFactory));

            var suffix = suffixFactory.Invoke(DateTime.Now);

            return entityType.AsSuffixTableSchema(suffix, schema);
        }

        /// <summary>
        /// 当作当前日期与时间偏移表架构（格式参考：names_suffix）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffixFactory">给定的 <see cref="DateTimeOffset"/> 后缀工厂方法（可选）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsDateTimeOffsetTableSchema(this Type entityType,
            Func<DateTimeOffset, string> suffixFactory, string schema = null)
        {
            suffixFactory.NotNull(nameof(suffixFactory));

            var suffix = suffixFactory.Invoke(DateTimeOffset.Now);

            return entityType.AsSuffixTableSchema(suffix, schema);
        }

        /// <summary>
        /// 当作后缀表架构（格式参考：names_?）。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="suffix">给定的表名后缀。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema AsSuffixTableSchema(this Type entityType,
            string suffix, string schema = null)
        {
            return entityType.AsTableSchema(names => $"{names}_{suffix}", schema);
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
            var names = TableSchema.GetEntityPluralName(entityType);

            if (namesFactory.IsNotNull())
                names = namesFactory.Invoke(names);

            return new TableSchema(names, schema);
        }

        #endregion

    }
}
