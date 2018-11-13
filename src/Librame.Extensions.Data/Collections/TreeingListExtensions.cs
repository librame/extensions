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
using System.Collections.Generic;

namespace Librame.Extensions
{
    using Data;

    /// <summary>
    /// 树形列表静态扩展。
    /// </summary>
    public static class TreeingListExtensions
    {
        
        /// <summary>
        /// 转换为树形化集合。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型。</typeparam>
        /// <param name="items">给定的项集合。</param>
        /// <returns>返回树形化接口。</returns>
        public static ITreeingList<T, int> AsTreeing<T>(this IEnumerable<T> items)
            where T : IParentId<int>
        {
            return new TreeingList<T, int>(items);
        }

        /// <summary>
        /// 转换为树形化集合。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="items">给定的项集合。</param>
        /// <returns>返回树形化接口。</returns>
        public static ITreeingList<T, TId> AsTreeing<T, TId>(this IEnumerable<T> items)
            where T : IParentId<TId>
            where TId : IEquatable<TId>
        {
            return new TreeingList<T, TId>(items);
        }

    }
}
