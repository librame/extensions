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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象发表（继承自抽象创建）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TPublishedBy">指定的发表者。</typeparam>
    [NotMapped]
    public abstract class AbstractPublication<TId, TPublishedBy> : AbstractPublication<TId, TPublishedBy, DateTimeOffset>,
        IPublication<TPublishedBy>
        where TId : IEquatable<TId>
        where TPublishedBy : IEquatable<TPublishedBy>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractPublication{TId, TPublishedBy}"/>。
        /// </summary>
        protected AbstractPublication()
        {
            PublishedTime = CreatedTime = DataSettings.Preference.DefaultCreatedTime;
            PublishedTimeTicks = CreatedTimeTicks = PublishedTime.Ticks;
        }


        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        [Display(Name = nameof(CreatedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual long CreatedTimeTicks { get; set; }

        /// <summary>
        /// 发表时间周期数。
        /// </summary>
        [Display(Name = nameof(PublishedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual long PublishedTimeTicks { get; set; }


        /// <summary>
        /// 异步设置创建时间。
        /// </summary>
        /// <param name="newCreatedTime">给定的新创建时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public override ValueTask<object> SetObjectCreatedTimeAsync(object newCreatedTime,
            CancellationToken cancellationToken = default)
        {
            var realNewCreatedTime = newCreatedTime.CastTo<object, DateTimeOffset>(nameof(newCreatedTime));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                CreatedTime = realNewCreatedTime;
                CreatedTimeTicks = CreatedTime.Ticks;
                return newCreatedTime;
            });
        }

        /// <summary>
        /// 异步设置发表时间。
        /// </summary>
        /// <param name="newPublishedTime">给定的新发表时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public override ValueTask<object> SetObjectPublishedTimeAsync(object newPublishedTime, CancellationToken cancellationToken = default)
        {
            var realNewPublishedTime = newPublishedTime.CastTo<object, DateTimeOffset>(nameof(newPublishedTime));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                PublishedTime = realNewPublishedTime;
                PublishedTimeTicks = PublishedTime.Ticks;
                return newPublishedTime;
            });
        }

    }


    /// <summary>
    /// 抽象发表（继承自抽象创建）。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TPublishedBy">指定的发表者。</typeparam>
    /// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    [NotMapped]
    public abstract class AbstractPublication<TId, TPublishedBy, TPublishedTime> : AbstractCreation<TId, TPublishedBy, TPublishedTime>,
        IPublication<TPublishedBy, TPublishedTime>
        where TId : IEquatable<TId>
        where TPublishedBy : IEquatable<TPublishedBy>
        where TPublishedTime : struct
    {
        /// <summary>
        /// 发表时间。
        /// </summary>
        [Display(Name = nameof(PublishedTime), ResourceType = typeof(AbstractEntityResource))]
        public virtual TPublishedTime PublishedTime { get; set; }

        /// <summary>
        /// 发表者。
        /// </summary>
        [Display(Name = nameof(PublishedBy), ResourceType = typeof(AbstractEntityResource))]
        public virtual TPublishedBy PublishedBy { get; set; }

        /// <summary>
        /// 发表为（如：资源链接）。
        /// </summary>
        [Display(Name = nameof(PublishedAs), ResourceType = typeof(AbstractEntityResource))]
        public virtual string PublishedAs { get; set; }


        /// <summary>
        /// 异步获取发表时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectPublishedTimeAsync(CancellationToken cancellationToken)
            => cancellationToken.RunOrCancelValueAsync(() => (object)PublishedTime);

        /// <summary>
        /// 异步获取发表者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectPublishedByAsync(CancellationToken cancellationToken)
            => cancellationToken.RunOrCancelValueAsync(() => (object)PublishedBy);


        /// <summary>
        /// 异步设置发表时间。
        /// </summary>
        /// <param name="newPublishedTime">给定的新发表时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectPublishedTimeAsync(object newPublishedTime,
            CancellationToken cancellationToken = default)
        {
            var realNewPublishedTime = newPublishedTime.CastTo<object, TPublishedTime>(nameof(newPublishedTime));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                PublishedTime = realNewPublishedTime;
                return newPublishedTime;
            });
        }

        /// <summary>
        /// 异步设置发表者。
        /// </summary>
        /// <param name="newPublishedBy">给定的新发表者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectPublishedByAsync(object newPublishedBy,
            CancellationToken cancellationToken = default)
        {
            var realNewPublishedBy = newPublishedBy.CastTo<object, TPublishedBy>(nameof(newPublishedBy));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                PublishedBy = realNewPublishedBy;
                return newPublishedBy;
            });
        }

    }
}
