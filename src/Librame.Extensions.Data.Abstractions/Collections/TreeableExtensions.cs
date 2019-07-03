﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace System.Collections.Generic
{
    /// <summary>
    /// <see cref="ITreeable{T, TId}"/> 静态扩展。
    /// </summary>
    public static class TreeableExtensions
    {
        /// <summary>
        /// 转换为可树形集合。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型。</typeparam>
        /// <param name="items">给定的项集合。</param>
        /// <returns>返回树形化接口。</returns>
        public static ITreeable<T> AsTreeing<T>(this IEnumerable<T> items)
            where T : IParentId<int>
        {
            return new Treeing<T>(items);
        }

        /// <summary>
        /// 转换为可树形集合。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="items">给定的项集合。</param>
        /// <returns>返回树形化接口。</returns>
        public static ITreeable<T, TId> AsTreeing<T, TId>(this IEnumerable<T> items)
            where T : IParentId<TId>
            where TId : IEquatable<TId>
        {
            return new Treeing<T, TId>(items);
        }

    }
}