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
    /// 发表静态扩展。
    /// </summary>
    public static class AbstractionPublicationExtensions
    {
        /// <summary>
        /// 异步填充发表属性（已集成填充创建属性）。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IPublication{TPublishedBy}"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<IPublication<TPublishedBy>> PopulatePublicationAsync<TPublishedBy>
            (this IPublication<TPublishedBy> publication, IClockService clock,
            CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
        {
            await publication.PopulateCreationAsync(clock, cancellationToken)
                .ConfigureAwait();

            publication.PublishedTime = publication.CreatedTime;
            publication.PublishedTimeTicks = publication.CreatedTimeTicks;

            return publication;
        }

        /// <summary>
        /// 异步填充发表属性（支持日期时间为可空类型；已集成填充创建属性）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="publication"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TPublication">指定的发表类型。</typeparam>
        /// <param name="publication">给定的 <typeparamref name="TPublication"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="newPublishedBy">给定的新发表者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublication"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<TPublication> PopulatePublicationAsync<TPublication>(this TPublication publication,
            IClockService clock, object newPublishedBy, CancellationToken cancellationToken = default)
            where TPublication : IObjectPublication
        {
            publication.NotNull(nameof(publication));

            var newPublishedTime = await StoreHelper.GetDateTimeNowAsync(clock,
                publication.CreatedTimeType, cancellationToken).ConfigureAwait();

            if (newPublishedTime.IsNotNull())
            {
                await publication.SetObjectCreatedTimeAsync(newPublishedTime, cancellationToken)
                    .ConfigureAwait();

                await publication.SetObjectPublishedTimeAsync(newPublishedTime, cancellationToken)
                    .ConfigureAwait();
            }

            await publication.SetObjectCreatedByAsync(newPublishedBy, cancellationToken)
                .ConfigureAwait();

            await publication.SetObjectPublishedByAsync(newPublishedBy, cancellationToken)
                .ConfigureAwait();

            return publication;
        }


        /// <summary>
        /// 异步获取发表时间。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublishedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TPublishedTime> GetPublishedTimeAsync<TPublishedBy, TPublishedTime>(
            this IPublication<TPublishedBy, TPublishedTime> publication, CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.NotNull(nameof(publication));

            return cancellationToken.RunOrCancelValueAsync(() => publication.PublishedTime);
        }

        /// <summary>
        /// 异步获取发表者。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublishedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TPublishedBy> GetPublishedByAsync<TPublishedBy, TPublishedTime>(
            this IPublication<TPublishedBy, TPublishedTime> publication, CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.NotNull(nameof(publication));

            return cancellationToken.RunOrCancelValueAsync(() => publication.PublishedBy);
        }


        /// <summary>
        /// 异步获取发表时间。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="newPublishedTimeFactory">给定的新发表时间工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublishedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TPublishedTime> SetPublishedTimeAsync<TPublishedBy, TPublishedTime>(
            this IPublication<TPublishedBy, TPublishedTime> publication, Func<TPublishedTime, TPublishedTime> newPublishedTimeFactory,
            CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.NotNull(nameof(publication));

            return cancellationToken.RunOrCancelValueAsync(()
                => publication.PublishedTime = newPublishedTimeFactory.Invoke(publication.PublishedTime));
        }

        /// <summary>
        /// 异步获取发表者。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="newPublishedByFactory">给定的新发表者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublishedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TPublishedBy> SetPublishedByAsync<TPublishedBy, TPublishedTime>(
            this IPublication<TPublishedBy, TPublishedTime> publication, Func<TPublishedBy, TPublishedBy> newPublishedByFactory,
            CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.NotNull(nameof(publication));

            return cancellationToken.RunOrCancelValueAsync(()
                => publication.PublishedBy = newPublishedByFactory.Invoke(publication.PublishedBy));
        }


        /// <summary>
        /// 异步获取发表时间。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="newPublishedTime">给定的新发表时间。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublishedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public static ValueTask<TPublishedTime> SetPublishedTimeAsync<TPublishedBy, TPublishedTime>(
            this IPublication<TPublishedBy, TPublishedTime> publication, TPublishedTime newPublishedTime,
            CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.NotNull(nameof(publication));

            return cancellationToken.RunOrCancelValueAsync(()
                => publication.PublishedTime = newPublishedTime);
        }

        /// <summary>
        /// 异步获取发表者。
        /// </summary>
        /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
        /// <typeparam name="TPublishedTime">指定的发表时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
        /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
        /// <param name="newPublishedBy">给定的新发表者。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TPublishedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public static ValueTask<TPublishedBy> SetPublishedByAsync<TPublishedBy, TPublishedTime>(
            this IPublication<TPublishedBy, TPublishedTime> publication, TPublishedBy newPublishedBy,
            CancellationToken cancellationToken = default)
            where TPublishedBy : IEquatable<TPublishedBy>
            where TPublishedTime : struct
        {
            publication.NotNull(nameof(publication));

            return cancellationToken.RunOrCancelValueAsync(()
                => publication.PublishedBy = newPublishedBy);
        }


        /// <summary>
        /// 异步设置对象发表时间。
        /// </summary>
        /// <param name="publication">给定的 <see cref="IObjectPublication"/>。</param>
        /// <param name="newPublishedTimeFactory">给定的新对象发表时间工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectPublishedTimeAsync(this IObjectPublication publication,
            Func<object, object> newPublishedTimeFactory, CancellationToken cancellationToken = default)
        {
            publication.NotNull(nameof(publication));
            newPublishedTimeFactory.NotNull(nameof(newPublishedTimeFactory));

            var newPublishedTime = await publication.GetObjectPublishedTimeAsync(cancellationToken).ConfigureAwait();
            return await publication.SetObjectPublishedTimeAsync(newPublishedTimeFactory.Invoke(newPublishedTime), cancellationToken)
                .ConfigureAwait();
        }

        /// <summary>
        /// 异步设置对象发表者。
        /// </summary>
        /// <param name="publication">给定的 <see cref="IObjectPublication"/>。</param>
        /// <param name="newPublishedByFactory">给定的新对象发表者工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectPublishedByAsync(this IObjectPublication publication,
            Func<object, object> newPublishedByFactory, CancellationToken cancellationToken = default)
        {
            publication.NotNull(nameof(publication));
            newPublishedByFactory.NotNull(nameof(newPublishedByFactory));

            var newPublishedBy = await publication.GetObjectPublishedByAsync(cancellationToken).ConfigureAwait();
            return await publication.SetObjectPublishedByAsync(newPublishedByFactory.Invoke(newPublishedBy), cancellationToken)
                .ConfigureAwait();
        }

    }
}
