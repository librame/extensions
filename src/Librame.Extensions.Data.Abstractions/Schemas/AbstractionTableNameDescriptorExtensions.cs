#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Globalization;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象表名描述符静态扩展。
    /// </summary>
    public static class AbstractionTableNameDescriptorExtensions
    {
        /// <summary>
        /// 转换为表名架构。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="TableNameSchema"/>。</returns>
        public static TableNameSchema AsSchema(this TableNameDescriptor descriptor, string schema = null)
            => new TableNameSchema(descriptor, schema);


        /// <summary>
        /// 改变日期后缀为年份（即按年份进行分表；参考：_19）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateSuffixByYear(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateSuffix(now => now.ToString("yy", CultureInfo.InvariantCulture));

        /// <summary>
        /// 改变日期后缀为年份月份（即按年月进行分表；参考：_1910）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateSuffixByYearMonth(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateSuffix(now => now.ToString("yyMM", CultureInfo.InvariantCulture));

        /// <summary>
        /// 改变日期后缀为年份周数（即按年周进行分表；参考：_1925）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateSuffixByYearWeek(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateSuffix(now => $"{now.ToString("yy", CultureInfo.InvariantCulture)}{now.AsWeekOfYear().FormatString(2)}");

        /// <summary>
        /// 改变日期后缀为年份季度（即按年季度进行分表；参考：_1903）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateSuffixByYearQuarter(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateSuffix(now => $"{now.ToString("yy", CultureInfo.InvariantCulture)}{now.AsQuarterOfYear().FormatString(2)}");


        /// <summary>
        /// 改变日期后缀为年份（即按年份进行分表；参考：_19）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateOffsetSuffixByYear(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateOffsetSuffix(now => now.ToString("yy", CultureInfo.InvariantCulture));

        /// <summary>
        /// 改变日期后缀为年份月份（即按年月进行分表；参考：_1910）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateOffsetSuffixByYearMonth(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateOffsetSuffix(now => now.ToString("yyMM", CultureInfo.InvariantCulture));

        /// <summary>
        /// 改变日期后缀为年份周数（即按年周进行分表；参考：_1925）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateOffsetSuffixByYearWeek(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateOffsetSuffix(now => $"{now.ToString("yy", CultureInfo.InvariantCulture)}{now.AsWeekOfYear().FormatString(2)}");

        /// <summary>
        /// 改变日期后缀为年份季度（即按年季度进行分表；参考：_1903）。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeDateOffsetSuffixByYearQuarter(this TableNameDescriptor descriptor)
            => descriptor?.ChangeDateOffsetSuffix(now => $"{now.ToString("yy", CultureInfo.InvariantCulture)}{now.AsQuarterOfYear().FormatString(2)}");
    }
}
