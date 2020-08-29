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
    using Core.Identifiers;

    /// <summary>
    /// 创建标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public interface ICreationIdentifier<TId, TCreatedBy> : ICreationIdentifier<TId, TCreatedBy, DateTimeOffset>,
        ICreation<TCreatedBy>
        where TId : IEquatable<TId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
    }


    /// <summary>
    /// 创建标识符接口。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface ICreationIdentifier<TId, TCreatedBy, TCreatedTime> : IIdentifier<TId>,
        ICreation<TCreatedBy, TCreatedTime>, IObjectCreationIdentifier
        where TId : IEquatable<TId>
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
    {
    }
}
