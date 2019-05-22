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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 更新接口。
    /// </summary>
    public interface IUpdation : ICreation
    {
        /// <summary>
        /// 获取更新时间。
        /// </summary>
        /// <returns>返回日期与时间（兼容 DateTime 或 DateTimeOffset）。</returns>
        object GetUpdatedTime();

        /// <summary>
        /// 获取更新者。
        /// </summary>
        /// <returns>返回更新者（兼容标识或字符串）。</returns>
        object GetUpdatedBy();
    }


    /// <summary>
    /// 更新接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface IUpdation<TId, TDateTime> : ICreation<TId, TDateTime>
        where TId : IEquatable<TId>
        where TDateTime : struct
    {
        /// <summary>
        /// 更新时间。
        /// </summary>
        TDateTime UpdatedTime { get; set; }

        /// <summary>
        /// 更新者。
        /// </summary>
        TId UpdatedBy { get; set; }
    }
}