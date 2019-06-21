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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 可处置对象接口。
    /// </summary>
    /// <typeparam name="TDisposable">指定的可处置对象类型。</typeparam>
    public interface IDisposable<TDisposable> : IDisposable
    {
    }
}
