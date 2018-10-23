#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 分页列表接口。
    /// </summary>
    /// <typeparam name="T">指定的分页类型。</typeparam>
    public interface IPagingList<T> : IEnumerable<T>, IList<T>
    {
        /// <summary>
        /// 行列表。
        /// </summary>
        IList<T> Rows { get; }

        /// <summary>
        /// 描述符。
        /// </summary>
        PagingDescriptor Descriptor { get; }
    }
}
