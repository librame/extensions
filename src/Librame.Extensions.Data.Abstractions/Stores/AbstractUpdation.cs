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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractUpdation<TId> : AbstractUpdation<TId, string, DateTimeOffset>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractUpdation{TId}"/>。
        /// </summary>
        public AbstractUpdation()
        {
            UpdatedTime = CreatedTime = EntityUtility.DefaultTime;
        }
    }


    /// <summary>
    /// 抽象更新。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
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
        /// 获取更新时间。
        /// </summary>
        /// <returns>返回日期与时间。</returns>
        public virtual object GetUpdatedTime()
            => UpdatedTime;

        /// <summary>
        /// 获取更新者。
        /// </summary>
        /// <returns>返回更新者。</returns>
        public virtual object GetUpdatedBy()
            => UpdatedBy;
    }
}
