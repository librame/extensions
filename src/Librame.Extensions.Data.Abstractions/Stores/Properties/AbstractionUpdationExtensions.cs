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
    /// 更新静态扩展。
    /// </summary>
    public static class AbstractionUpdationExtensions
    {
        /// <summary>
        /// 填充更新属性（已集成填充创建属性）。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的发表者类型。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回 <see cref="IUpdation{TUpdatedBy}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IUpdation<TUpdatedBy> PopulateUpdation<TUpdatedBy>
            (this IUpdation<TUpdatedBy> updation, IClockService clock)
            where TUpdatedBy : IEquatable<TUpdatedBy>
        {
            updation.PopulateCreation(clock);

            updation.UpdatedTime = updation.CreatedTime;
            updation.UpdatedTimeTicks = updation.CreatedTimeTicks;

            return updation;
        }

        /// <summary>
        /// 异步填充更新属性（已集成填充创建属性）。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的发表者类型。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IUpdation{TUpdatedBy}"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<IUpdation<TUpdatedBy>> PopulateUpdationAsync<TUpdatedBy>
            (this IUpdation<TUpdatedBy> updation, IClockService clock,
            CancellationToken cancellationToken = default)
            where TUpdatedBy : IEquatable<TUpdatedBy>
        {
            await updation.PopulateCreationAsync(clock, cancellationToken)
                .ConfigureAwait();

            updation.UpdatedTime = updation.CreatedTime;
            updation.UpdatedTimeTicks = updation.CreatedTimeTicks;

            return updation;
        }


        /// <summary>
        /// 填充更新属性（支持日期时间为可空类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="updation"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TUpdation">指定的更新类型。</typeparam>
        /// <param name="updation">给定的 <typeparamref name="TUpdation"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="newUpdatedBy">给定的新更新者对象。</param>
        /// <returns>返回 <typeparamref name="TUpdation"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TUpdation PopulateUpdation<TUpdation>(this TUpdation updation,
            IClockService clock, object newUpdatedBy)
            where TUpdation : IObjectUpdation
        {
            updation.NotNull(nameof(updation));

            var newUpdatedTime = StoreHelper.GetDateTimeNow(clock, updation.CreatedTimeType);
            if (newUpdatedTime.IsNotNull())
            {
                updation.SetObjectCreatedTime(newUpdatedTime);
                updation.SetObjectUpdatedTime(newUpdatedTime);
            }

            updation.SetObjectCreatedBy(newUpdatedBy);
            updation.SetObjectUpdatedBy(newUpdatedBy);

            return updation;
        }

        /// <summary>
        /// 异步填充更新属性（支持日期时间为可空类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="updation"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TUpdation">指定的更新类型。</typeparam>
        /// <param name="updation">给定的 <typeparamref name="TUpdation"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="newUpdatedBy">给定的新更新者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdation"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<TUpdation> PopulateUpdationAsync<TUpdation>(this TUpdation updation,
            IClockService clock, object newUpdatedBy, CancellationToken cancellationToken = default)
            where TUpdation : IObjectUpdation
        {
            updation.NotNull(nameof(updation));

            var newUpdatedTime = await StoreHelper.GetDateTimeNowAsync(clock,
                updation.CreatedTimeType, cancellationToken).ConfigureAwait();

            if (newUpdatedTime.IsNotNull())
            {
                await updation.SetObjectCreatedTimeAsync(newUpdatedTime, cancellationToken)
                    .ConfigureAwait();

                await updation.SetObjectUpdatedTimeAsync(newUpdatedTime, cancellationToken)
                    .ConfigureAwait();
            }

            await updation.SetObjectCreatedByAsync(newUpdatedBy, cancellationToken)
                .ConfigureAwait();

            await updation.SetObjectUpdatedByAsync(newUpdatedBy, cancellationToken)
                .ConfigureAwait();

            return updation;
        }


        /// <summary>
        /// 获取更新时间。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
        /// <typeparam name="TUpdatedTime">指定的更新时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</param>
        /// <param name="newUpdatedTimeFactory">给定的新更新时间工厂方法。</param>
        /// <returns>返回 <typeparamref name="TUpdatedTime"/>（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TUpdatedTime SetUpdatedTime<TUpdatedBy, TUpdatedTime>
            (this IUpdation<TUpdatedBy, TUpdatedTime> updation,
            Func<TUpdatedTime, TUpdatedTime> newUpdatedTimeFactory)
            where TUpdatedBy : IEquatable<TUpdatedBy>
            where TUpdatedTime : struct
        {
            updation.NotNull(nameof(updation));
            return updation.UpdatedTime = newUpdatedTimeFactory.Invoke(updation.UpdatedTime);
        }

        /// <summary>
        /// 异步获取更新时间。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
        /// <typeparam name="TUpdatedTime">指定的更新时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</param>
        /// <param name="newUpdatedTimeFactory">给定的新更新时间工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdatedTime"/>（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TUpdatedTime> SetUpdatedTimeAsync<TUpdatedBy, TUpdatedTime>
            (this IUpdation<TUpdatedBy, TUpdatedTime> updation,
            Func<TUpdatedTime, TUpdatedTime> newUpdatedTimeFactory,
            CancellationToken cancellationToken = default)
            where TUpdatedBy : IEquatable<TUpdatedBy>
            where TUpdatedTime : struct
        {
            updation.NotNull(nameof(updation));

            return cancellationToken.RunOrCancelValueAsync(()
                => updation.UpdatedTime = newUpdatedTimeFactory.Invoke(updation.UpdatedTime));
        }


        /// <summary>
        /// 获取更新者。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
        /// <typeparam name="TUpdatedTime">指定的更新时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</param>
        /// <param name="newUpdatedByFactory">给定的新更新者工厂方法。</param>
        /// <returns>返回 <typeparamref name="TUpdatedBy"/>（兼容标识或字符串）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TUpdatedBy SetUpdatedBy<TUpdatedBy, TUpdatedTime>
            (this IUpdation<TUpdatedBy, TUpdatedTime> updation,
            Func<TUpdatedBy, TUpdatedBy> newUpdatedByFactory)
            where TUpdatedBy : IEquatable<TUpdatedBy>
            where TUpdatedTime : struct
        {
            updation.NotNull(nameof(updation));
            return updation.UpdatedBy = newUpdatedByFactory.Invoke(updation.UpdatedBy);
        }

        /// <summary>
        /// 异步获取更新者。
        /// </summary>
        /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
        /// <typeparam name="TUpdatedTime">指定的更新时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</param>
        /// <param name="newUpdatedByFactory">给定的新更新者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdatedBy"/>（兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TUpdatedBy> SetUpdatedByAsync<TUpdatedBy, TUpdatedTime>
            (this IUpdation<TUpdatedBy, TUpdatedTime> updation,
            Func<TUpdatedBy, TUpdatedBy> newUpdatedByFactory,
            CancellationToken cancellationToken = default)
            where TUpdatedBy : IEquatable<TUpdatedBy>
            where TUpdatedTime : struct
        {
            updation.NotNull(nameof(updation));

            return cancellationToken.RunOrCancelValueAsync(()
                => updation.UpdatedBy = newUpdatedByFactory.Invoke(updation.UpdatedBy));
        }


        /// <summary>
        /// 设置对象更新时间。
        /// </summary>
        /// <param name="updation">给定的 <see cref="IObjectUpdation"/>。</param>
        /// <param name="newUpdatedTimeFactory">给定的新对象更新时间工厂方法。</param>
        /// <returns>返回更新时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object SetObjectUpdatedTime(this IObjectUpdation updation,
            Func<object, object> newUpdatedTimeFactory)
        {
            updation.NotNull(nameof(updation));
            newUpdatedTimeFactory.NotNull(nameof(newUpdatedTimeFactory));

            var newUpdatedTime = updation.GetObjectUpdatedTimeAsync();
            return updation.SetObjectUpdatedTime(newUpdatedTimeFactory.Invoke(newUpdatedTime));
        }

        /// <summary>
        /// 异步设置对象更新时间。
        /// </summary>
        /// <param name="updation">给定的 <see cref="IObjectUpdation"/>。</param>
        /// <param name="newUpdatedTimeFactory">给定的新对象更新时间工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含更新时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectUpdatedTimeAsync(this IObjectUpdation updation,
            Func<object, object> newUpdatedTimeFactory, CancellationToken cancellationToken = default)
        {
            updation.NotNull(nameof(updation));
            newUpdatedTimeFactory.NotNull(nameof(newUpdatedTimeFactory));

            var newUpdatedTime = await updation.GetObjectUpdatedTimeAsync(cancellationToken).ConfigureAwait();
            return await updation.SetObjectUpdatedTimeAsync(newUpdatedTimeFactory.Invoke(newUpdatedTime), cancellationToken)
                .ConfigureAwait();
        }


        /// <summary>
        /// 设置对象更新者。
        /// </summary>
        /// <param name="updation">给定的 <see cref="IObjectUpdation"/>。</param>
        /// <param name="newUpdatedByFactory">给定的新对象更新者工厂方法。</param>
        /// <returns>返回更新者（兼容标识或字符串）。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object SetObjectUpdatedBy(this IObjectUpdation updation,
            Func<object, object> newUpdatedByFactory)
        {
            updation.NotNull(nameof(updation));
            newUpdatedByFactory.NotNull(nameof(newUpdatedByFactory));

            var newUpdatedBy = updation.GetObjectUpdatedBy();
            return updation.SetObjectUpdatedBy(newUpdatedByFactory.Invoke(newUpdatedBy));
        }

        /// <summary>
        /// 异步设置对象更新者。
        /// </summary>
        /// <param name="updation">给定的 <see cref="IObjectUpdation"/>。</param>
        /// <param name="newUpdatedByFactory">给定的新对象更新者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectUpdatedByAsync(this IObjectUpdation updation,
            Func<object, object> newUpdatedByFactory, CancellationToken cancellationToken = default)
        {
            updation.NotNull(nameof(updation));
            newUpdatedByFactory.NotNull(nameof(newUpdatedByFactory));

            var newUpdatedBy = await updation.GetObjectUpdatedByAsync(cancellationToken).ConfigureAwait();
            return await updation.SetObjectUpdatedByAsync(newUpdatedByFactory.Invoke(newUpdatedBy), cancellationToken)
                .ConfigureAwait();
        }

    }
}
