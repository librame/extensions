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
            CreatedTime = DataSettings.UtcNowOffset;
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
    public abstract class AbstractCreation<TId, TCreatedBy, TCreatedTime> : AbstractId<TId>, ICreation<TCreatedBy, TCreatedTime>
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
        /// 获取自定义创建时间。
        /// </summary>
        /// <returns>返回日期与时间。</returns>
        public virtual object GetCustomCreatedTime()
            => CreatedTime;

        /// <summary>
        /// 获取自定义创建者。
        /// </summary>
        /// <returns>返回创建者。</returns>
        public virtual object GetCustomCreatedBy()
            => CreatedBy;
    }
}
