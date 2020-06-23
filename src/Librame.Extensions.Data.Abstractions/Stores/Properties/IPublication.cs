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
    /// 发表接口（已集成创建接口）。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    public interface IPublication<TPublishedBy> : IPublication<TPublishedBy, DateTimeOffset>, IPublishedTimeTicks, ICreation<TPublishedBy>
        where TPublishedBy : IEquatable<TPublishedBy>
    {
    }


    /// <summary>
    /// 发表接口（已集成创建接口）。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    /// <typeparam name="TPublishedTime">指定的发表日期与时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface IPublication<TPublishedBy, TPublishedTime> : ICreation<TPublishedBy, TPublishedTime>, IObjectPublication
        where TPublishedBy : IEquatable<TPublishedBy>
        where TPublishedTime : struct
    {
        /// <summary>
        /// 发表时间。
        /// </summary>
        TPublishedTime PublishedTime { get; set; }

        /// <summary>
        /// 发表者。
        /// </summary>
        TPublishedBy PublishedBy { get; set; }

        /// <summary>
        /// 发表为（如：资源链接）。
        /// </summary>
        string PublishedAs { get; set; }
    }
}
