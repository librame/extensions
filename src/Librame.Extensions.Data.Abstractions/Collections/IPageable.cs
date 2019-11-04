#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// 可分页接口。
    /// </summary>
    /// <typeparam name="T">指定的分页类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IPageable<T> : IEnumerable<T>, IPagingInfo
    {
        /// <summary>
        /// 描述符。
        /// </summary>
        PagingDescriptor Descriptor { get; }
    }
}
