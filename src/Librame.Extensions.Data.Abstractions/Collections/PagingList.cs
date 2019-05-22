#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 分页列表。
    /// </summary>
    /// <typeparam name="T">指定的分页类型。</typeparam>
    public class PagingList<T> : IPagingList<T>, IEnumerable<T>, IList<T>
    {
        /// <summary>
        /// 构造一个 <see cref="PagingList{T}"/> 实例。
        /// </summary>
        /// <param name="query">给定的查询接口。</param>
        /// <param name="descriptor">给定的描述符。</param>
        public PagingList(IQueryable<T> query, PagingDescriptor descriptor)
        {
            query.NotNull(nameof(query));

            // 跳过条数
            if (descriptor.Skip > 0)
                query = query.Skip(descriptor.Skip);

            // 获取条数
            if (descriptor.Size > 0)
                query = query.Take(descriptor.Size);

            Rows = query.ToList();
            Descriptor = descriptor;
        }

        /// <summary>
        /// 构造一个 <see cref="PagingList{T}"/> 实例。
        /// </summary>
        /// <param name="rows">给定的行集合。</param>
        /// <param name="descriptor">给定的描述符。</param>
        public PagingList(IList<T> rows, PagingDescriptor descriptor)
        {
            rows.NotNull(nameof(rows));

            if (rows.Count > descriptor.Size)
            {
                // 跳过条数
                if (descriptor.Skip > 0)
                    rows = rows.Skip(descriptor.Skip).ToList();

                // 获取条数
                if (descriptor.Size > 0)
                    rows = rows.Take(descriptor.Size).ToList();
            }

            Rows = rows;
            Descriptor = descriptor;
        }

        
        /// <summary>
        /// 行列表。
        /// </summary>
        public virtual IList<T> Rows { get; }

        /// <summary>
        /// 描述符。
        /// </summary>
        public virtual PagingDescriptor Descriptor { get; }


        #region IEnumerable<T> Members

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

        #endregion


        #region IList<T> Members

        /// <summary>
        /// 元素数。
        /// </summary>
        public int Count => Rows.Count;

        /// <summary>
        /// 是否为只读。
        /// </summary>
        public bool IsReadOnly => Rows.IsReadOnly;

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        /// <returns>返回树形节点。</returns>
        public T this[int index]
        {
            get { return Rows[index]; }
            set { Rows[index] = value; }
        }


        /// <summary>
        /// 添加指定的树形节点。
        /// </summary>
        /// <param name="item">给定的树形节点。</param>
        public void Add(T item)
        {
            Rows.Add(item);
        }


        /// <summary>
        /// 清空所有树形节点。
        /// </summary>
        public void Clear()
        {
            Rows.Clear();
        }


        /// <summary>
        /// 是否包含指定的树形节点。
        /// </summary>
        /// <param name="item">给定的树形节点。</param>
        /// <returns>返回布尔值。</returns>
        public bool Contains(T item)
        {
            return Rows.Contains(item);
        }


        /// <summary>
        /// 从指定的索引开始，将现有树形节点集合复制到新的数组中。
        /// </summary>
        /// <param name="array">给定的新数组。</param>
        /// <param name="arrayIndex">指定的索引。</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Rows.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// 将项插入指定列表的索引处。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        /// <param name="item">给定的项。</param>
        public void Insert(int index, T item)
        {
            Rows.Insert(index, item);
        }

        /// <summary>
        /// 获取指定项所在列表中的索引。
        /// </summary>
        /// <param name="item">指定的树形节点。</param>
        /// <returns>返回整数。</returns>
        public int IndexOf(T item)
        {
            return Rows.IndexOf(item);
        }


        /// <summary>
        /// 移除指定树形节点。
        /// </summary>
        /// <param name="item">给定的树形节点。</param>
        /// <returns>返回布尔值。</returns>
        public bool Remove(T item)
        {
            return Rows.Remove(item);
        }

        /// <summary>
        /// 移除指定索引处的项。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        public void RemoveAt(int index)
        {
            Rows.RemoveAt(index);
        }

        #endregion

    }
}
