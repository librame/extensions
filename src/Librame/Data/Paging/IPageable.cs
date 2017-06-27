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
    /// 公开分页数，该分页数支持在指定类型的集合上进行简单分页。
    /// </summary>
    /// <typeparam name="T">要分页的对象的类型。</typeparam>
    public interface IPageable<T> : IEnumerable<T>
    {
        /// <summary>
        /// 获取行列表。
        /// </summary>
        IList<T> Rows { get; }

        /// <summary>
        /// 获取分页信息。
        /// </summary>
        PagingInfo Info { get; }


        /// <summary>
        /// 更新当前可分页集合的类型实例。
        /// </summary>
        /// <param name="selector">给定的选择器。</param>
        /// <returns>返回结果类型的公开分页数。</returns>
        IPageable<T> UpdatePaging(Func<T, T> selector);

        /// <summary>
        /// 转换为与当前分页信息相同的结果类型实例的可分页集合。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="selector">给定的选择器。</param>
        /// <returns>返回结果类型的公开分页数。</returns>
        IPageable<TResult> AsPaging<TResult>(Func<T, TResult> selector);
    }
}
