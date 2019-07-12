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
    /// 生成式标识接口（默认标识类型为 <see cref="string"/>）。
    /// </summary>
    public interface IGenId : IGenId<string>
    {
    }


    /// <summary>
    /// 生成式标识接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public interface IGenId<TGenId> : IId<TGenId>
        where TGenId : IEquatable<TGenId>
    {
    }
}
