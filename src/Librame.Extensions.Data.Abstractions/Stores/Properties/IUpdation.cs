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
    /// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface IUpdation<TUpdatedBy, TUpdatedTime> : ICreation<TUpdatedBy, TUpdatedTime>
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


    /// <summary>
    /// 更新接口。
    /// </summary>
    public interface IUpdation : ICreation
    {
        /// <summary>
        /// 获取自定义更新时间。
        /// </summary>
        /// <returns>返回日期与时间（兼容 DateTime 或 DateTimeOffset）。</returns>
        object GetCustomUpdatedTime();

        /// <summary>
        /// 获取自定义更新者。
        /// </summary>
        /// <returns>返回更新者（兼容标识或字符串）。</returns>
        object GetCustomUpdatedBy();
    }
}