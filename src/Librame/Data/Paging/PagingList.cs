#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Utility;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
    /// <summary>
    /// 分页列表。
    /// </summary>
    /// <typeparam name="T">要分页的对象类型。</typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class PagingList<T> : IPagingable<T>
    {
        /// <summary>
        /// 获取行列表。
        /// </summary>
        public virtual IList<T> Rows { get; }

        /// <summary>
        /// 获取分页信息。
        /// </summary>
        public virtual PagingInfo Info { get; }

        
        /// <summary>
        /// 构造一个 <see cref="PagingList{T}"/> 实例。
        /// </summary>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="info">给定的分页信息。</param>
        public PagingList(IList<T> rows, PagingInfo info)
        {
            Rows = rows.NotNull(nameof(rows));
            Info = info.NotNull(nameof(info));
        }


        /// <summary>
        /// 更新当前可分页集合的类型实例。
        /// </summary>
        /// <param name="selector">给定的选择器。</param>
        /// <returns>返回结果类型的公开分页数。</returns>
        public virtual IPagingable<T> UpdatePaging(Func<T, T> selector)
        {
            return AsPaging(selector);
        }

        /// <summary>
        /// 转换为与当前分页信息相同的结果类型实例的可分页集合。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="selector">给定的选择器。</param>
        /// <returns>返回结果类型的公开分页数。</returns>
        public virtual IPagingable<TResult> AsPaging<TResult>(Func<T, TResult> selector)
        {
            try
            {
                var changeRows = Rows.Select(selector).ToList();

                return new PagingList<TResult>(changeRows, Info);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>返回枚举器。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Rows.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
