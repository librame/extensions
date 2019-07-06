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
    /// 创建接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TDateTime">指定的日期与时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface ICreation<TId, TDateTime> : ICreation
        where TId : IEquatable<TId>
        where TDateTime : struct
    {
        /// <summary>
        /// 创建时间。
        /// </summary>
        TDateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        TId CreatedBy { get; set; }
    }


    /// <summary>
    /// 创建接口。
    /// </summary>
    public interface ICreation
    {
        /// <summary>
        /// 获取创建时间。
        /// </summary>
        /// <returns>返回日期与时间（兼容 DateTime 或 DateTimeOffset）。</returns>
        object GetCreatedTime();

        /// <summary>
        /// 获取创建者。
        /// </summary>
        /// <returns>返回创建者（兼容标识或字符串）。</returns>
        object GetCreatedBy();
    }
}