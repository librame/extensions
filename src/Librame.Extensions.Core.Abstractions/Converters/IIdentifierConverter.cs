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
    /// 标识符转换器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IIdentifierConverter<T> : IConverter<ReadOnlyMemory<T>, string>
    {
    }
}
