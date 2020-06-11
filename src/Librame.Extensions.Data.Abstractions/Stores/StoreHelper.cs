#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 存储助手。
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 按类型名称创建（默认为 <see cref="IDataPreferenceSetting.ViewCreatedByTypeNameMaxLength"/>，超过部分以省略号代替）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string CreatedByTypeName<T>()
            => CreatedByTypeName(typeof(T));

        /// <summary>
        /// 按类型名称创建。
        /// </summary>
        /// <param name="referenceType">给定的引用类型。</param>
        /// <param name="viewMaxLength">查看的最大长度（默认为 <see cref="IDataPreferenceSetting.ViewCreatedByTypeNameMaxLength"/>，超过部分以省略号代替）。</param>
        /// <returns>返回字符串。</returns>
        public static string CreatedByTypeName(Type referenceType, int? viewMaxLength = null)
        {
            var name = referenceType.GetDisplayNameWithNamespace();

            if (viewMaxLength.IsNull())
                viewMaxLength = DataSettings.Preference.ViewCreatedByTypeNameMaxLength;

            if (name.Length > viewMaxLength)
                name = $"{referenceType.GetGenericBodyName()}...";

            return name;
        }


        /// <summary>
        /// 填充默认租户。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="defaultTenant"/> is null.
        /// </exception>
        /// <param name="defaultTenant">给定的 <see cref="ITenant"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static ITenant PopulateDefaultTenant(ITenant defaultTenant)
        {
            defaultTenant.NotNull(nameof(defaultTenant));

            if (defaultTenant.Name.IsEmpty())
                defaultTenant.Name = "DefaultTenant";

            if (defaultTenant.Host.IsEmpty())
                defaultTenant.Host = "localhost";

            if (defaultTenant.DefaultConnectionString.IsEmpty())
                defaultTenant.DefaultConnectionString = "librame_data_default";

            if (defaultTenant.WritingConnectionString.IsEmpty())
                defaultTenant.WritingConnectionString = "librame_data_writing";

            return defaultTenant;
        }


        /// <summary>
        /// 更新日期时间对象（支持日期时间为可空类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="clock"/> or <paramref name="dateTimeType"/> is null.
        /// </exception>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="dateTimeType">给定的日期时间类型。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回更新或默认日期时间。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static async Task<object> GetDateTimeNowAsync(IClockService clock, Type dateTimeType,
            CancellationToken cancellationToken = default)
        {
            clock.NotNull(nameof(clock));
            dateTimeType.NotNull(nameof(dateTimeType));

            if (dateTimeType.IsGenericType
                && dateTimeType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                dateTimeType = dateTimeType.UnwrapNullableType();
            }

            if (dateTimeType == typeof(DateTime))
                return await clock.GetNowAsync(cancellationToken: cancellationToken).ConfigureAndResultAsync();

            else if (dateTimeType == typeof(DateTimeOffset))
                return await clock.GetNowOffsetAsync(cancellationToken: cancellationToken).ConfigureAndResultAsync();

            return null;
        }

    }
}
