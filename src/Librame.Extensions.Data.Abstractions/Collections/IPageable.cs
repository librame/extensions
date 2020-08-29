#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Collections
{
    /// <summary>
    /// 可分页接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IPageable<T> : IEnumerable<T>, IPagingInfo
    {
        /// <summary>
        /// 分页描述符。
        /// </summary>
        PagingDescriptor Descriptor { get; }
    }
}
