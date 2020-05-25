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
    /// 抽象更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractUpdation<TId> : AbstractUpdation<TId, string, DateTimeOffset>, IUpdatedTimeTicks
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractUpdation{TId}"/>。
        /// </summary>
        protected AbstractUpdation()
        {
            UpdatedTime = CreatedTime = DataSettings.Preference.DefaultCreatedTime;
            UpdatedTimeTicks = CreatedTimeTicks = UpdatedTime.Ticks;
        }


        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        [Display(Name = nameof(CreatedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual long CreatedTimeTicks { get; set; }

        /// <summary>
        /// 更新时间周期数。
        /// </summary>
        [Display(Name = nameof(UpdatedTimeTicks), ResourceType = typeof(AbstractEntityResource))]
        public virtual long UpdatedTimeTicks { get; set; }
    }


    /// <summary>
    /// 抽象更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    [NotMapped]
    public abstract class AbstractUpdation<TId, TUpdatedBy, TUpdatedTime> : AbstractCreation<TId, TUpdatedBy, TUpdatedTime>, IUpdation<TUpdatedBy, TUpdatedTime>
        where TId : IEquatable<TId>
        where TUpdatedBy : IEquatable<TUpdatedBy>
        where TUpdatedTime : struct
    {
        /// <summary>
        /// 更新者。
        /// </summary>
        [Display(Name = nameof(UpdatedBy), ResourceType = typeof(AbstractEntityResource))]
        public virtual TUpdatedBy UpdatedBy { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        [Display(Name = nameof(UpdatedTime), ResourceType = typeof(AbstractEntityResource))]
        public virtual TUpdatedTime UpdatedTime { get; set; }


        /// <summary>
        /// 异步获取更新者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdatedBy"/> （兼容标识或字符串）的异步操作。</returns>
        public virtual Task<TUpdatedBy> GetUpdatedByAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => UpdatedBy);

        Task<object> IUpdation.GetUpdatedByAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)UpdatedBy);

        /// <summary>
        /// 异步获取更新时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TUpdatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        public virtual Task<TUpdatedTime> GetUpdatedTimeAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => UpdatedTime);

        Task<object> IUpdation.GetUpdatedTimeAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)UpdatedTime);


        /// <summary>
        /// 异步设置更新者。
        /// </summary>
        /// <param name="updatedBy">给定的 <typeparamref name="TUpdatedBy"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetUpdatedByAsync(TUpdatedBy updatedBy, CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => UpdatedBy = updatedBy);

        /// <summary>
        /// 异步设置更新者。
        /// </summary>
        /// <param name="obj">给定的更新者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetUpdatedByAsync(object obj, CancellationToken cancellationToken = default)
        {
            var updatedBy = obj.CastTo<object, TUpdatedBy>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => UpdatedBy = updatedBy);
        }

        /// <summary>
        /// 异步设置更新时间。
        /// </summary>
        /// <param name="updatedTime">给定的 <typeparamref name="TUpdatedTime"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetUpdatedTimeAsync(TUpdatedTime updatedTime, CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => UpdatedTime = updatedTime);

        /// <summary>
        /// 异步设置更新时间。
        /// </summary>
        /// <param name="obj">给定的更新时间对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetUpdatedTimeAsync(object obj, CancellationToken cancellationToken = default)
        {
            var updatedTime = obj.CastTo<object, TUpdatedTime>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => UpdatedTime = updatedTime);
        }

    }
}
