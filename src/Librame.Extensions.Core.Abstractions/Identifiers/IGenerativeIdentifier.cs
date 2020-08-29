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

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 生成式标识符接口。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型（如：<see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型）。</typeparam>
    public interface IGenerativeIdentifier<TGenId> : IIdentifier<TGenId>
        where TGenId : IEquatable<TGenId>
    {
    }
}
