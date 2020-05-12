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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象创建。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractCreation<TId> : AbstractCreation<TId, string, DateTimeOffset>, ICreatedTimeTicks
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractCreation{TId}"/>。
        /// </summary>
        protected AbstractCreation()
        {
            CreatedTime = DataSettings.Preference.DefaultCreatedTime;
            CreatedTimeTicks = CreatedTime.Ticks;
        }


        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        [Display(Name = nameof(CreatedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual long CreatedTimeTicks { get; set; }
    }


    /// <summary>
    /// 抽象创建。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    [NotMapped]
    public abstract class AbstractCreation<TId, TCreatedBy, TCreatedTime> : AbstractIdentifier<TId>, ICreation<TCreatedBy, TCreatedTime>
        where TId : IEquatable<TId>
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
    {
        /// <summary>
        /// 创建时间。
        /// </summary>
        [Display(Name = nameof(CreatedTime), ResourceType = typeof(AbstractEntityResource))]
        public virtual TCreatedTime CreatedTime { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        [Display(Name = nameof(CreatedBy), ResourceType = typeof(AbstractEntityResource))]
        public virtual TCreatedBy CreatedBy { get; set; }


        /// <summary>
        /// 异步获取创建者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public virtual Task<TCreatedBy> GetCreatedByAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => CreatedBy);

        Task<object> ICreation.GetCreatedByAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)CreatedBy);

        /// <summary>
        /// 异步获取创建时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TCreatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public virtual Task<TCreatedTime> GetCreatedTimeAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => CreatedTime);

        Task<object> ICreation.GetCreatedTimeAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)CreatedTime);


        /// <summary>
        /// 异步设置创建者。
        /// </summary>
        /// <param name="createdBy">给定的 <typeparamref name="TCreatedBy"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetCreatedByAsync(TCreatedBy createdBy, CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => CreatedBy = createdBy);

        /// <summary>
        /// 异步设置创建者。
        /// </summary>
        /// <param name="obj">给定的创建者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetCreatedByAsync(object obj, CancellationToken cancellationToken = default)
        {
            var createdBy = obj.CastTo<object, TCreatedBy>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => CreatedBy = createdBy);
        }

        /// <summary>
        /// 异步设置创建时间。
        /// </summary>
        /// <param name="createdTime">给定的 <typeparamref name="TCreatedTime"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetCreatedTimeAsync(TCreatedTime createdTime, CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => CreatedTime = createdTime);

        /// <summary>
        /// 异步设置创建时间。
        /// </summary>
        /// <param name="obj">给定的创建时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetCreatedTimeAsync(object obj, CancellationToken cancellationToken = default)
        {
            var createdTime = obj.CastTo<object, TCreatedTime>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => CreatedTime = createdTime);
        }

    }
}
