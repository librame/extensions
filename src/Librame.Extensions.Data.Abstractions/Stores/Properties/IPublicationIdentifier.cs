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
    /// 发表标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    public interface IPublicationIdentifier<TId, TPublishedBy> : IPublicationIdentifier<TId, TPublishedBy, DateTimeOffset>,
        IPublication<TPublishedBy>
        where TId : IEquatable<TId>
        where TPublishedBy : IEquatable<TPublishedBy>
    {
    }


    /// <summary>
    /// 发表标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    /// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface IPublicationIdentifier<TId, TPublishedBy, TPublishedTime>
        : ICreationIdentifier<TId, TPublishedBy, TPublishedTime>
        , IPublication<TPublishedBy, TPublishedTime>, IObjectPublicationIdentifier
        where TId : IEquatable<TId>
        where TPublishedBy : IEquatable<TPublishedBy>
        where TPublishedTime : struct
    {
    }
}
