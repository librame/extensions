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

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 更新接口（已集成创建接口）。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    public interface IUpdation<TUpdatedBy> : IUpdation<TUpdatedBy, DateTimeOffset>, IUpdatedTimeTicks, ICreation<TUpdatedBy>
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
    }


    /// <summary>
    /// 更新接口（已集成创建接口）。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface IUpdation<TUpdatedBy, TUpdatedTime> : ICreation<TUpdatedBy, TUpdatedTime>, IObjectUpdation
        where TUpdatedBy : IEquatable<TUpdatedBy>
        where TUpdatedTime : struct
    {
        /// <summary>
        /// 更新时间。
        /// </summary>
        TUpdatedTime UpdatedTime { get; set; }

        /// <summary>
        /// 更新者。
        /// </summary>
        TUpdatedBy UpdatedBy { get; set; }
    }
}