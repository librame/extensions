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
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
    public interface ICreation<TCreatedBy, TCreatedTime> : ICreation
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
    {
        /// <summary>
        /// 创建时间。
        /// </summary>
        TCreatedTime CreatedTime { get; set; }

        /// <summary>
        /// 创建者。
        /// </summary>
        TCreatedBy CreatedBy { get; set; }
    }


    /// <summary>
    /// 创建接口。
    /// </summary>
    public interface ICreation
    {
        /// <summary>
        /// 获取自定义创建时间。
        /// </summary>
        /// <returns>返回日期与时间（兼容 DateTime 或 DateTimeOffset）。</returns>
        object GetCustomCreatedTime();

        /// <summary>
        /// 获取自定义创建者。
        /// </summary>
        /// <returns>返回创建者（兼容标识或字符串）。</returns>
        object GetCustomCreatedBy();
    }
}