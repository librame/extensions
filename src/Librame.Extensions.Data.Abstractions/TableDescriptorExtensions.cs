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
using System.Globalization;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="TableDescriptor"/> 静态扩展。
    /// </summary>
    public static class TableDescriptorExtensions
    {
        private const string DefaultYearFormat = "yy";
        private const string DefaultYearAndMonthFormat = DefaultYearFormat + "MM";
        private const string DefaultTimestampFormat = "yyMMdd";


        #region Prefix

        /// <summary>
        /// 插入数据标记前缀（如：Data_）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor InsertDataPrefix(this TableDescriptor table)
            => table.InsertPrefix(nameof(Data));

        /// <summary>
        /// 插入私有标记前缀（如：__）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor InsertPrivatePrefix(this TableDescriptor table)
            => table.InsertPrefix(TableDescriptor.DefautlNameConnector);

        /// <summary>
        /// 插入前缀。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="prefix">给定的前缀。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor InsertPrefix(this TableDescriptor table, string prefix)
            => table?.ChangeName(name => $"{prefix.NotEmpty(nameof(prefix))}{table.NameConnector}{name}");

        #endregion


        #region Suffix

        /// <summary>
        /// 附加年份与周数后缀（如：_2051）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearAndWeekSuffix(this TableDescriptor table, DateTime timestamp)
        {
            var suffix = timestamp.ToString(DefaultYearFormat, CultureInfo.InvariantCulture);
            return table.AppendSuffix(suffix + timestamp.AsWeekOfYear().FormatString(2));
        }

        /// <summary>
        /// 附加年份与周数后缀（如：_2051）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearAndWeekSuffix(this TableDescriptor table, DateTimeOffset timestamp)
        {
            var suffix = timestamp.ToString(DefaultYearFormat, CultureInfo.InvariantCulture);
            return table.AppendSuffix(suffix + timestamp.AsWeekOfYear().FormatString(2));
        }


        /// <summary>
        /// 附加年份与季度后缀（如：_2003）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearAndQuarterSuffix(this TableDescriptor table, DateTime timestamp)
        {
            var suffix = timestamp.ToString(DefaultYearFormat, CultureInfo.InvariantCulture);
            return table.AppendSuffix(suffix + timestamp.AsQuarterOfYear().FormatString(2));
        }

        /// <summary>
        /// 附加年份与季度后缀（如：_2003）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearAndQuarterSuffix(this TableDescriptor table, DateTimeOffset timestamp)
        {
            var suffix = timestamp.ToString(DefaultYearFormat, CultureInfo.InvariantCulture);
            return table.AppendSuffix(suffix + timestamp.AsQuarterOfYear().FormatString(2));
        }


        /// <summary>
        /// 附加年份后缀（如：_20）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearSuffix(this TableDescriptor table, DateTime timestamp)
            => table.AppendTimestampSuffix(timestamp, DefaultYearFormat);

        /// <summary>
        /// 附加年份后缀（如：_20）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearSuffix(this TableDescriptor table, DateTimeOffset timestamp)
            => table.AppendTimestampSuffix(timestamp, DefaultYearFormat);


        /// <summary>
        /// 附加年份与月份后缀（如：_2009）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearAndMonthSuffix(this TableDescriptor table, DateTime timestamp)
            => table.AppendTimestampSuffix(timestamp, DefaultYearAndMonthFormat);

        /// <summary>
        /// 附加年份与月份后缀（如：_2009）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendYearAndMonthSuffix(this TableDescriptor table, DateTimeOffset timestamp)
            => table.AppendTimestampSuffix(timestamp, DefaultYearAndMonthFormat);


        /// <summary>
        /// 附加时间戳后缀（通常用于生成含有日期与时间的表名；如：_200410）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <param name="format">给定的格式化方式（可选；默认为 <see cref="DefaultTimestampFormat"/>）。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendTimestampSuffix(this TableDescriptor table, DateTime timestamp, string format = null)
        {
            var suffix = timestamp.ToString(format.NotEmptyOrDefault(DefaultTimestampFormat, throwIfDefaultInvalid: false),
                CultureInfo.InvariantCulture);

            return table.AppendSuffix(suffix);
        }

        /// <summary>
        /// 附加时间戳后缀（通常用于生成含有日期与时间的表名；如：_200410）。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <param name="format">给定的格式化方式（可选；默认为 <see cref="DefaultTimestampFormat"/>）。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendTimestampSuffix(this TableDescriptor table, DateTimeOffset timestamp, string format = null)
        {
            var suffix = timestamp.ToString(format.NotEmptyOrDefault(DefaultTimestampFormat, throwIfDefaultInvalid: false),
                CultureInfo.InvariantCulture);

            return table.AppendSuffix(suffix);
        }


        /// <summary>
        /// 附加后缀。
        /// </summary>
        /// <param name="table">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="suffix">给定的后缀。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor AppendSuffix(this TableDescriptor table, string suffix)
            => table?.ChangeName(name => $"{name}{table.NameConnector}{suffix.NotEmpty(nameof(suffix))}");

        #endregion

    }
}
