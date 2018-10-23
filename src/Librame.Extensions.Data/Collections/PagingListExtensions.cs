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
    /// 分页列表静态扩展。
    /// </summary>
    public static class PagingListExtensions
    {
        
        /// <summary>
        /// 创建分页列表。
        /// </summary>
        /// <typeparam name="T">指定的分页类型。</typeparam>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="descriptor">给定的分页描述符。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public static IPagingList<T> AsPaging<T>(this IList<T> rows, PagingDescriptor descriptor)
        {
            return new PagingList<T>(rows, descriptor);
        }

    }
}
