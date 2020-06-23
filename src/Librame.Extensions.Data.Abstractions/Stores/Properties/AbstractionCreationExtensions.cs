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
    /// 创建静态扩展。
    /// </summary>
    public static class AbstractionCreationExtensions
    {
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
                .ConfigureAndResultAsync();

            creation.CreatedTimeTicks = creation.CreatedTime.Ticks;

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
                creation.CreatedTimeType, cancellationToken).ConfigureAndResultAsync();

            if (newCreatedTime.IsNotNull())
            {
                await creation.SetObjectCreatedTimeAsync(newCreatedTime, cancellationToken)
                    .ConfigureAndResultAsync();
            }

            await creation.SetObjectCreatedByAsync(newCreatedBy, cancellationToken)
                .ConfigureAndResultAsync();

            return creation;
        }


        /// <summary>
        /// 异步获取创建时间。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TCreatedTime> GetCreatedTimeAsync<TCreatedBy, TCreatedTime>(
            this ICreation<TCreatedBy, TCreatedTime> creation, CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunFactoryOrCancellationValueAsync(() => creation.CreatedTime);
        }

        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TCreatedBy> GetCreatedByAsync<TCreatedBy, TCreatedTime>(
            this ICreation<TCreatedBy, TCreatedTime> creation, CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunFactoryOrCancellationValueAsync(() => creation.CreatedBy);
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
        public static ValueTask<TCreatedTime> SetCreatedTimeAsync<TCreatedBy, TCreatedTime>(
            this ICreation<TCreatedBy, TCreatedTime> creation, Func<TCreatedTime, TCreatedTime> newCreatedTimeFactory,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunFactoryOrCancellationValueAsync(()
                => creation.CreatedTime = newCreatedTimeFactory.Invoke(creation.CreatedTime));
        }

        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedByFactory">给定的新创建者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TCreatedBy> SetCreatedByAsync<TCreatedBy, TCreatedTime>(
            this ICreation<TCreatedBy, TCreatedTime> creation, Func<TCreatedBy, TCreatedBy> newCreatedByFactory,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunFactoryOrCancellationValueAsync(()
                => creation.CreatedBy = newCreatedByFactory.Invoke(creation.CreatedBy));
        }


        /// <summary>
        /// 异步获取创建时间。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedTime">给定的新创建时间。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TCreatedTime> SetCreatedTimeAsync<TCreatedBy, TCreatedTime>(
            this ICreation<TCreatedBy, TCreatedTime> creation, TCreatedTime newCreatedTime,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunFactoryOrCancellationValueAsync(()
                => creation.CreatedTime = newCreatedTime);
        }

        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
        /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
        /// <param name="newCreatedBy">给定的新创建者。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TCreatedBy> SetCreatedByAsync<TCreatedBy, TCreatedTime>(
            this ICreation<TCreatedBy, TCreatedTime> creation, TCreatedBy newCreatedBy,
            CancellationToken cancellationToken = default)
            where TCreatedBy : IEquatable<TCreatedBy>
            where TCreatedTime : struct
        {
            creation.NotNull(nameof(creation));

            return cancellationToken.RunFactoryOrCancellationValueAsync(()
                => creation.CreatedBy = newCreatedBy);
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

            var newCreatedTime = await creation.GetObjectCreatedTimeAsync(cancellationToken).ConfigureAndResultAsync();
            return await creation.SetObjectCreatedTimeAsync(newCreatedTimeFactory.Invoke(newCreatedTime), cancellationToken)
                .ConfigureAndResultAsync();
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

            var newCreatedBy = await creation.GetObjectCreatedByAsync(cancellationToken).ConfigureAndResultAsync();
            return await creation.SetObjectCreatedByAsync(newCreatedByFactory.Invoke(newCreatedBy), cancellationToken)
                .ConfigureAndResultAsync();
        }

    }
}
