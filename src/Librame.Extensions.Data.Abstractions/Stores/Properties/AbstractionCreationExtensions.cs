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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 创建静态扩展。
    /// </summary>
    public static class AbstractionCreationExtensions
    {
        /// <summary>
        /// 填充创建属性。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回 <see cref="ICreation{TCreatedBy}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ICreation<TCreatedBy> PopulateCreation<TCreatedBy>
            (this ICreation<TCreatedBy> creation, IClockService clock)
            where TCreatedBy : IEquatable<TCreatedBy>
        {
            creation.NotNull(nameof(creation));
            clock.NotNull(nameof(clock));

            creation.CreatedTime = clock.GetNowOffset();
            creation.CreatedTimeTicks = creation.CreatedTime.Ticks;

            return creation;
        }

        /// <summary>
        /// 异步填充创建属性。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="ICreation{TCreatedBy}"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<ICreation<TCreatedBy>> PopulateCreationAsync<TCreatedBy>
            (this ICreation<TCreatedBy> creation, IClockService clock,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
        {
            creation.NotNull(nameof(creation));
            clock.NotNull(nameof(clock));

            creation.CreatedTime = await clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                .ConfigureAwait();

            creation.CreatedTimeTicks = creation.CreatedTime.Ticks;

            return creation;
        }


        /// <summary>
        /// 填充创建属性（支持日期时间为可空类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="creation"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TCreation">指定的创建类型。</typeparam>
        /// <param name="creation">给定的 <typeparamref name="TCreation"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="newCreatedBy">给定的新创建者对象。</param>
        /// <returns>返回 <typeparamref name="TCreation"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TCreation PopulateCreation<TCreation>(this TCreation creation,
            IClockService clock, object newCreatedBy)
            where TCreation : IObjectCreation
        {
            creation.NotNull(nameof(creation));

            var newCreatedTime = StoreHelper.GetDateTimeNow(clock, creation.CreatedTimeType);
            if (newCreatedTime.IsNotNull())
            {
                creation.SetObjectCreatedTime(newCreatedTime);
            }

            creation.SetObjectCreatedBy(newCreatedBy);

            return creation;
        }

        /// <summary>
        /// 异步填充创建属性（支持日期时间为可空类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="creation"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TCreation">指定的创建类型。</typeparam>
        /// <param name="creation">给定的 <typeparamref name="TCreation"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="newCreatedBy">给定的新创建者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreation"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<TCreation> PopulateCreationAsync<TCreation>(this TCreation creation,
            IClockService clock, object newCreatedBy, CancellationToken cancellationToken = default)
            where TCreation : IObjectCreation
        {
            creation.NotNull(nameof(creation));

            var newCreatedTime = await StoreHelper.GetDateTimeNowAsync(clock,
                creation.CreatedTimeType, cancellationToken).ConfigureAwait();

            if (newCreatedTime.IsNotNull())
            {
                await creation.SetObjectCreatedTimeAsync(newCreatedTime, cancellationToken)
                    .ConfigureAwait();
            }

            await creation.SetObjectCreatedByAsync(newCreatedBy, cancellationToken)
                .ConfigureAwait();

            return creation;
        }


        /// <summary>
        /// 获取创建时间。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedTimeFactory">给定的新创建时间工厂方法。</param>
        /// <returns>返回 <typeparamref name="TCreatedTime"/>（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TCreatedTime SetCreatedTime<TCreatedBy, TCreatedTime>
            (this ICreation<TCreatedBy, TCreatedTime> creation, Func<TCreatedTime, TCreatedTime> newCreatedTimeFactory)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));
            newCreatedTimeFactory.NotNull(nameof(newCreatedTimeFactory));

            return creation.CreatedTime = newCreatedTimeFactory.Invoke(creation.CreatedTime);
        }

        /// <summary>
        /// 异步获取创建时间。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedTimeFactory">给定的新创建时间工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TCreatedTime> SetCreatedTimeAsync<TCreatedBy, TCreatedTime>
            (this ICreation<TCreatedBy, TCreatedTime> creation, Func<TCreatedTime, TCreatedTime> newCreatedTimeFactory,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));
            newCreatedTimeFactory.NotNull(nameof(newCreatedTimeFactory));

            return cancellationToken.RunOrCancelValueAsync(()
                => creation.CreatedTime = newCreatedTimeFactory.Invoke(creation.CreatedTime));
        }


        /// <summary>
        /// 获取创建者。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedByFactory">给定的新创建者工厂方法。</param>
        /// <returns>返回 <typeparamref name="TCreatedBy"/>（兼容标识或字符串）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TCreatedBy SetCreatedBy<TCreatedBy, TCreatedTime>
            (this ICreation<TCreatedBy, TCreatedTime> creation, Func<TCreatedBy, TCreatedBy> newCreatedByFactory)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));
            newCreatedByFactory.NotNull(nameof(newCreatedByFactory));

            return creation.CreatedBy = newCreatedByFactory.Invoke(creation.CreatedBy);
        }

        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedByFactory">给定的新创建者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/>（兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TCreatedBy> SetCreatedByAsync<TCreatedBy, TCreatedTime>
            (this ICreation<TCreatedBy, TCreatedTime> creation, Func<TCreatedBy, TCreatedBy> newCreatedByFactory,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunOrCancelValueAsync(()
                => creation.CreatedBy = newCreatedByFactory.Invoke(creation.CreatedBy));
        }


        /// <summary>
        /// 设置对象创建时间。
        /// </summary>
        /// <param name="creation">给定的 <see cref="IObjectCreation"/>。</param>
        /// <param name="newCreatedTimeFactory">给定的新对象创建时间工厂方法。</param>
        /// <returns>返回创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object SetObjectCreatedTime(this IObjectCreation creation,
            Func<object, object> newCreatedTimeFactory)
        {
            creation.NotNull(nameof(creation));
            newCreatedTimeFactory.NotNull(nameof(newCreatedTimeFactory));

            var newCreatedTime = creation.GetObjectCreatedTime();
            return creation.SetObjectCreatedTime(newCreatedTimeFactory.Invoke(newCreatedTime));
        }

        /// <summary>
        /// 异步设置对象创建时间。
        /// </summary>
        /// <param name="creation">给定的 <see cref="IObjectCreation"/>。</param>
        /// <param name="newCreatedTimeFactory">给定的新对象创建时间工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectCreatedTimeAsync(this IObjectCreation creation,
            Func<object, object> newCreatedTimeFactory, CancellationToken cancellationToken = default)
        {
            creation.NotNull(nameof(creation));
            newCreatedTimeFactory.NotNull(nameof(newCreatedTimeFactory));

            var newCreatedTime = await creation.GetObjectCreatedTimeAsync(cancellationToken).ConfigureAwait();
            return await creation.SetObjectCreatedTimeAsync(newCreatedTimeFactory.Invoke(newCreatedTime), cancellationToken)
                .ConfigureAwait();
        }


        /// <summary>
        /// 设置对象创建者。
        /// </summary>
        /// <param name="creation">给定的 <see cref="IObjectCreation"/>。</param>
        /// <param name="newCreatedByFactory">给定的新对象创建者工厂方法。</param>
        /// <returns>返回创建者（兼容标识或字符串）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object SetObjectCreatedBy(this IObjectCreation creation,
            Func<object, object> newCreatedByFactory)
        {
            creation.NotNull(nameof(creation));
            newCreatedByFactory.NotNull(nameof(newCreatedByFactory));

            var newCreatedBy = creation.GetObjectCreatedBy();
            return creation.SetObjectCreatedBy(newCreatedByFactory.Invoke(newCreatedBy));
        }

        /// <summary>
        /// 异步设置对象创建者。
        /// </summary>
        /// <param name="creation">给定的 <see cref="IObjectCreation"/>。</param>
        /// <param name="newCreatedByFactory">给定的新对象创建者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectCreatedByAsync(this IObjectCreation creation,
            Func<object, object> newCreatedByFactory, CancellationToken cancellationToken = default)
        {
            creation.NotNull(nameof(creation));
            newCreatedByFactory.NotNull(nameof(newCreatedByFactory));

            var newCreatedBy = await creation.GetObjectCreatedByAsync(cancellationToken).ConfigureAwait();
            return await creation.SetObjectCreatedByAsync(newCreatedByFactory.Invoke(newCreatedBy), cancellationToken)
                .ConfigureAwait();
        }


        /// <summary>
        /// 导入创建。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型（支持所有整数、<see cref="Guid"/> 类型）。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="by">给定的字符串使为者。</param>
        /// <param name="time">给定的字符串时间。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static void ImportCreation<TCreatedBy, TCreatedTime>(this ICreation<TCreatedBy, TCreatedTime> creation,
            string by, string time, IFormatProvider provider = null)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            creation.CreatedBy = by.ToCreatedBy<TCreatedBy>(provider);
            creation.CreatedTime = time.ToCreatedTime<TCreatedTime>(provider);
        }

        /// <summary>
        /// 导入发表。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型（支持所有整数、<see cref="Guid"/> 类型）。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="by">给定的字符串使为者。</param>
        /// <param name="time">给定的字符串时间。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static void ImportPublication<TPublishedBy, TPublishedTime>(this IPublication<TPublishedBy, TPublishedTime> publication,
            string by, string time, IFormatProvider provider = null)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.ImportCreation(by, time, provider);

            publication.PublishedBy = by.ToCreatedBy<TPublishedBy>(provider);
            publication.PublishedTime = time.ToCreatedTime<TPublishedTime>(provider);
        }

        /// <summary>
        /// 导入更新。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的更新者类型（支持所有整数、<see cref="Guid"/> 类型）。</typeparam>
        /// <typeparam name="TUpdatedTime">指定的更新时间类型（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型）。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</param>
        /// <param name="by">给定的字符串使为者。</param>
        /// <param name="time">给定的字符串时间。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static void ImportUpdation<TUpdatedBy, TUpdatedTime>(this IUpdation<TUpdatedBy, TUpdatedTime> updation,
            string by, string time, IFormatProvider provider = null)
            where TUpdatedBy : IEquatable<TUpdatedBy>
            where TUpdatedTime : struct
        {
            updation.ImportCreation(by, time, provider);

            updation.UpdatedBy = by.ToCreatedBy<TUpdatedBy>(provider);
            updation.UpdatedTime = time.ToCreatedTime<TUpdatedTime>(provider);
        }


        /// <summary>
        /// 转为创建者（支持所有整数、<see cref="Guid"/> 类型创建者的字符串形式）。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型（支持所有整数、<see cref="Guid"/> 类型）。</typeparam>
        /// <param name="by">给定的字符串使为者。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回 <typeparamref name="TCreatedBy"/>。</returns>
        public static TCreatedBy ToCreatedBy<TCreatedBy>(this string by, IFormatProvider provider = null)
            where TCreatedBy : IEquatable<TCreatedBy>
            => (TCreatedBy)by.ToCreatedBy(typeof(TCreatedBy), provider);

        /// <summary>
        /// 转为创建者（支持所有整数、<see cref="Guid"/> 类型创建者的字符串形式）。
        /// </summary>
        /// <param name="by">给定的字符串使为者。</param>
        /// <param name="byType">给定的使为者类型（支持所有整数、<see cref="Guid"/> 类型）。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回使为者对象。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object ToCreatedBy(this string by, Type byType, IFormatProvider provider = null)
        {
            byType.NotNull(nameof(byType));

            if (provider.IsNull())
                provider = CultureInfo.InvariantCulture;

            object obj = byType.Name switch
            {
                "Guid" => Guid.Parse(by),
                "SByte" => sbyte.Parse(by, provider),
                "Byte" => byte.Parse(by, provider),
                "Int16" => short.Parse(by, provider),
                "UInt16" => ushort.Parse(by, provider),
                "Int32" => int.Parse(by, provider),
                "UInt32" => uint.Parse(by, provider),
                "Int64" => long.Parse(by, provider),
                "UInt64" => ulong.Parse(by, provider),
                _ => new NotSupportedException()
            };

            return obj;
        }


        /// <summary>
        /// 转为创建时间（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型时间的字符串形式）。
        /// </summary>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型）。</typeparam>
        /// <param name="time">给定的字符串时间。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回 <typeparamref name="TCreatedTime"/>。</returns>
        public static TCreatedTime ToCreatedTime<TCreatedTime>(this string time, IFormatProvider provider = null)
            where TCreatedTime : struct
            => (TCreatedTime)time.ToCreatedTime(typeof(TCreatedTime), provider);

        /// <summary>
        /// 转为创建时间（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型时间的字符串形式）。
        /// </summary>
        /// <param name="time">给定的字符串时间。</param>
        /// <param name="timeType">给定的时间类型（支持 <see cref="DateTime"/>、<see cref="DateTimeOffset"/>、长整形周期数等类型）。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回时间对象。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object ToCreatedTime(this string time, Type timeType, IFormatProvider provider = null)
        {
            timeType.NotNull(nameof(timeType));

            if (provider.IsNull())
                provider = CultureInfo.InvariantCulture;

            object obj = timeType.Name switch
            {
                "DateTime" => DateTime.Parse(time, provider),
                "DateTimeOffset" => DateTimeOffset.Parse(time, provider),
                "Int64" => new DateTime(long.Parse(time, provider)),
                _ => new NotSupportedException()
            };

            return obj;
        }

    }
}
