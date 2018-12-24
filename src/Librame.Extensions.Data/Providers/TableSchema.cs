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
    /// 表架构。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public class TableSchema<TEntity> : TableSchema
    {
        private static readonly Type _entityType = typeof(TEntity);


        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableSchema(string schema = null)
            : base(GetTypeNames(), schema)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        /// <param name="nameFactory">给定的命名工厂方法（输入参数为实体类型名称的复数形式）。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableSchema(Func<string, string> nameFactory, string schema = null)
            : base(nameFactory.Invoke(GetTypeNames()), schema)
        {
        }


        /// <summary>
        /// 构建内部表架构（命名规则参考：__TypeNames）。
        /// </summary>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildInternal()
        {
            return new TableSchema<TEntity>(typeNames => $"__{typeNames}");
        }


        /// <summary>
        /// 构建以每天为规则的表架构（命名规则参考：TypeNames_YearMonthDay）。
        /// </summary>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonthDay(DateTime dateTime, string schema = null)
        {
            return BuildEveryYearMonthDay(dateTime, _entityType, schema);
        }
        /// <summary>
        /// 构建以每天为规则的表架构（命名规则参考：TypeNames_YearMonthDay）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonthDay(DateTimeOffset dateTimeOffset, string schema = null)
        {
            return BuildEveryYearMonthDay(dateTimeOffset, _entityType, schema);
        }

        /// <summary>
        /// 构建以每周为规则的表架构（命名规则参考：TypeNames_YearWeek）。
        /// </summary>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearWeek(DateTime dateTime, string schema = null)
        {
            return BuildEveryYearWeek(dateTime, _entityType, schema);
        }
        /// <summary>
        /// 构建以每周为规则的表架构（命名规则参考：TypeNames_YearWeek）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearWeek(DateTimeOffset dateTimeOffset, string schema = null)
        {
            return BuildEveryYearWeek(dateTimeOffset, _entityType, schema);
        }

        /// <summary>
        /// 构建以每月为规则的表架构（命名规则参考：TypeNames_YearMonth）。
        /// </summary>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonth(DateTime dateTime, string schema = null)
        {
            return BuildEveryYearMonth(dateTime, _entityType, schema);
        }
        /// <summary>
        /// 构建以每月为规则的表架构（命名规则参考：TypeNames_YearMonth）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonth(DateTimeOffset dateTimeOffset, string schema = null)
        {
            return BuildEveryYearMonth(dateTimeOffset, _entityType, schema);
        }

        /// <summary>
        /// 构建以每季度为规则的表架构（命名规则参考：TypeNames_YearQuarter）。
        /// </summary>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearQuarter(DateTime dateTime, string schema = null)
        {
            return BuildEveryYearQuarter(dateTime, _entityType, schema);
        }
        /// <summary>
        /// 构建以每季度为规则的表架构（命名规则参考：TypeNames_YearQuarter）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearQuarter(DateTimeOffset dateTimeOffset, string schema = null)
        {
            return BuildEveryYearQuarter(dateTimeOffset, _entityType, schema);
        }

        /// <summary>
        /// 构建以每年为规则的表架构（命名规则参考：TypeNames_Year）。
        /// </summary>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYear(DateTime dateTime, string schema = null)
        {
            return BuildEveryYear(dateTime, _entityType, schema);
        }
        /// <summary>
        /// 构建以每年为规则的表架构（命名规则参考：TypeNames_Year）。
        /// </summary>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYear(DateTimeOffset dateTimeOffset, string schema = null)
        {
            return BuildEveryYear(dateTimeOffset, _entityType, schema);
        }


        /// <summary>
        /// 获取类型名复数形式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public static string GetTypeNames()
        {
            return GetTypeNames(_entityType);
        }
    }


    /// <summary>
    /// 表架构。
    /// </summary>
    public class TableSchema : ITableSchema
    {
        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableSchema(Type entityType, string schema = null)
            : this(GetTypeNames(entityType), schema)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// name is empty.
        /// </exception>
        /// <param name="name">给定的名称。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableSchema(string name, string schema = null)
        {
            Name = name.NotEmpty(nameof(name));
            Schema = schema;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 架构。
        /// </summary>
        public string Schema { get; set; }


        /// <summary>
        /// 构建以每天为规则的表架构（命名规则参考：TypeNames_YearMonthDay）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonthDay(DateTime dateTime, Type entityType, string schema = null)
        {
            var everyDay = dateTime.ToString("yyMMdd");

            return new TableSchema($"{GetTypeNames(entityType)}_{everyDay}", schema);
        }
        /// <summary>
        /// 构建以每天为规则的表架构（命名规则参考：TypeNames_YearMonthDay）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonthDay(DateTimeOffset dateTimeOffset, Type entityType, string schema = null)
        {
            var everyDay = dateTimeOffset.ToString("yyMMdd");

            return new TableSchema($"{GetTypeNames(entityType)}_{everyDay}", schema);
        }

        /// <summary>
        /// 构建以每周为规则的表架构（命名规则参考：TypeNames_YearWeek）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearWeek(DateTime dateTime, Type entityType, string schema = null)
        {
            var everyWeek = dateTime.ToString("yy") + dateTime.AsWeekOfYear();

            return new TableSchema($"{GetTypeNames(entityType)}_{everyWeek}", schema);
        }
        /// <summary>
        /// 构建以每周为规则的表架构（命名规则参考：TypeNames_YearWeek）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearWeek(DateTimeOffset dateTimeOffset, Type entityType, string schema = null)
        {
            var everyWeek = dateTimeOffset.ToString("yy") + dateTimeOffset.AsWeekOfYear();

            return new TableSchema($"{GetTypeNames(entityType)}_{everyWeek}", schema);
        }

        /// <summary>
        /// 构建以每月为规则的表架构（命名规则参考：TypeNames_YearMonth）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonth(DateTime dateTime, Type entityType, string schema = null)
        {
            var everyMonth = dateTime.ToString("yyMM");

            return new TableSchema($"{GetTypeNames(entityType)}_{everyMonth}", schema);
        }
        /// <summary>
        /// 构建以每月为规则的表架构（命名规则参考：TypeNames_YearMonth）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearMonth(DateTimeOffset dateTimeOffset, Type entityType, string schema = null)
        {
            var everyMonth = dateTimeOffset.ToString("yyMM");

            return new TableSchema($"{GetTypeNames(entityType)}_{everyMonth}", schema);
        }

        /// <summary>
        /// 构建以每季度为规则的表架构（命名规则参考：TypeNames_YearQuarter）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearQuarter(DateTime dateTime, Type entityType, string schema = null)
        {
            var everyQuarter = dateTime.ToString("yy") + dateTime.AsQuarterOfYear();

            return new TableSchema($"{GetTypeNames(entityType)}_{everyQuarter}", schema);
        }
        /// <summary>
        /// 构建以每季度为规则的表架构（命名规则参考：TypeNames_YearQuarter）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYearQuarter(DateTimeOffset dateTimeOffset, Type entityType, string schema = null)
        {
            var everyQuarter = dateTimeOffset.ToString("yy") + dateTimeOffset.AsQuarterOfYear();

            return new TableSchema($"{GetTypeNames(entityType)}_{everyQuarter}", schema);
        }

        /// <summary>
        /// 构建以每年为规则的表架构（命名规则参考：TypeNames_Year）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTime">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYear(DateTime dateTime, Type entityType, string schema = null)
        {
            var everyYear = dateTime.ToString("yy");

            return new TableSchema($"{GetTypeNames(entityType)}_{everyYear}", schema);
        }
        /// <summary>
        /// 构建以每年为规则的表架构（命名规则参考：TypeNames_Year）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="dateTimeOffset">给定的日期与时间。</param>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public static ITableSchema BuildEveryYear(DateTimeOffset dateTimeOffset, Type entityType, string schema = null)
        {
            var everyYear = dateTimeOffset.ToString("yy");

            return new TableSchema($"{GetTypeNames(entityType)}_{everyYear}", schema);
        }


        /// <summary>
        /// 获取类型名复数形式。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// entityType is null.
        /// </exception>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetTypeNames(Type entityType)
        {
            entityType.NotDefault(nameof(entityType));

            var name = entityType.Name;

            if (entityType.IsGenericType)
                name = entityType.Name.SplitPair("`").Key;

            return name.AsPluralize();
        }
    }
}
