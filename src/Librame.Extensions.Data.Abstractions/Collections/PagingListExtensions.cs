#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace System.Collections.Generic
{
    /// <summary>
    /// 分页列表静态扩展。
    /// </summary>
    public static class PagingListExtensions
    {
        /// <summary>
        /// 当作分页列表。
        /// </summary>
        /// <typeparam name="T">指定的分页类型。</typeparam>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的页大小。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public static IPagingList<T> AsPagingListByIndex<T>(this IList<T> rows, int index, int size)
        {
            return new PagingList<T>(rows, PagingDescriptor.CreateByIndex(index, size, rows.Count));
        }

        /// <summary>
        /// 当作分页列表。
        /// </summary>
        /// <typeparam name="T">指定的分页类型。</typeparam>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="skip">给定的跳过条数。</param>
        /// <param name="take">给定的获取条数。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public static IPagingList<T> AsPagingListBySkip<T>(this IList<T> rows, int skip, int take)
        {
            return new PagingList<T>(rows, PagingDescriptor.CreateBySkip(skip, take, rows.Count));
        }

        /// <summary>
        /// 当作分页列表。
        /// </summary>
        /// <typeparam name="T">指定的分页类型。</typeparam>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="descriptor">给定的分页描述符。</param>
        /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
        public static IPagingList<T> AsPagingList<T>(this IList<T> rows, PagingDescriptor descriptor)
        {
            return new PagingList<T>(rows, descriptor);
        }

    }
}
