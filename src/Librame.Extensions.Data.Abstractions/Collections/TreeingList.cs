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
using Librame.Extensions.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 树状列表。
    /// </summary>
    /// <typeparam name="T">指定的树状元素类型。</typeparam>
    /// <typeparam name="TId">指定的树状元素标识类型。</typeparam>
    [NotMapped]
    public class TreeingList<T, TId> : ITreeingList<T, TId>, IEnumerable<TreeingNode<T, TId>>, IList<TreeingNode<T, TId>>
        where T : IParentId<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 节点列表。
        /// </summary>
        public IList<TreeingNode<T, TId>> Nodes { get; }

        /// <summary>
        /// 无层级节点列表。
        /// </summary>
        public IList<TreeingNode<T, TId>> NonstratifiedNodes { get; private set; }

        
        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/> 实例。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        /// <param name="orderBy">给定的排序工厂方法（可选）。</param>
        public TreeingList(IEnumerable<T> items,
            Func<IEnumerable<TreeingNode<T, TId>>, IOrderedEnumerable<TreeingNode<T, TId>>> orderBy = null)
        {
            items.NotNull(nameof(items));

            Nodes = ToNodes(items);
            NonstratifiedNodes = ToNonstratifiedNodes(Nodes, orderBy);
        }
        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/> 实例。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        /// <param name="orderBy">给定的排序工厂方法（可选）。</param>
        public TreeingList(IList<TreeingNode<T, TId>> nodes,
            Func<IEnumerable<TreeingNode<T, TId>>, IOrderedEnumerable<TreeingNode<T, TId>>> orderBy = null)
        {
            Nodes = nodes.NotNull(nameof(nodes));
            NonstratifiedNodes = ToNonstratifiedNodes(nodes, orderBy);
        }


        /// <summary>
        /// 转换为树形节点列表。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        /// <returns>返回树形节点列表。</returns>
        protected virtual IList<TreeingNode<T, TId>> ToNodes(IEnumerable<T> items)
        {
            return items.AsTreeingNodes<T, TId>();
        }

        /// <summary>
        /// 转换为无层级树形节点列表。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        /// <param name="orderBy">给定的排序工厂方法（可选）。</param>
        /// <returns>返回无层级树形节点列表。</returns>
        protected virtual IList<TreeingNode<T, TId>> ToNonstratifiedNodes(IList<TreeingNode<T, TId>> nodes,
            Func<IEnumerable<TreeingNode<T, TId>>, IOrderedEnumerable<TreeingNode<T, TId>>> orderBy = null)
        {
            List<TreeingNode<T, TId>> nonstratifiedNodes = null;

            if (nodes.IsNotNull() && nodes.Count > 0)
            {
                nonstratifiedNodes = new List<TreeingNode<T, TId>>();

                foreach (var n in nodes)
                {
                    nonstratifiedNodes.Add(n);

                    if (n.HasChildren)
                    {
                        // 链式查询
                        var lookupNodes = ToNonstratifiedNodes(n.Children);

                        orderBy?.Invoke(lookupNodes);

                        nonstratifiedNodes.AddRange(lookupNodes);
                    }
                }
            }

            return nonstratifiedNodes;
        }


        #region IEnumerable<TreeingNode<T, TId>> Members

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>返回枚举数。</returns>
        public IEnumerator<TreeingNode<T, TId>> GetEnumerator()
        {
            if (Nodes.IsNullOrEmpty()) return null;

            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        #region IList<TreeingNode<T, TId>> Members

        /// <summary>
        /// 元素数。
        /// </summary>
        public int Count => Nodes.Count;

        /// <summary>
        /// 是否为只读。
        /// </summary>
        public bool IsReadOnly => Nodes.IsReadOnly;

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        /// <returns>返回树形节点。</returns>
        public TreeingNode<T, TId> this[int index]
        {
            get { return Nodes[index]; }
            set { Nodes[index] = value; }
        }


        /// <summary>
        /// 添加指定的树形节点。
        /// </summary>
        /// <param name="item">给定的树形节点。</param>
        public void Add(TreeingNode<T, TId> item)
        {
            Nodes.Add(item);
        }


        /// <summary>
        /// 清空所有树形节点。
        /// </summary>
        public void Clear()
        {
            Nodes.Clear();
        }


        /// <summary>
        /// 是否包含指定的树形节点。
        /// </summary>
        /// <param name="item">给定的树形节点。</param>
        /// <returns>返回布尔值。</returns>
        public bool Contains(TreeingNode<T, TId> item)
        {
            return Nodes.Contains(item);
        }


        /// <summary>
        /// 从指定的索引开始，将现有树形节点集合复制到新的数组中。
        /// </summary>
        /// <param name="array">给定的新数组。</param>
        /// <param name="arrayIndex">指定的索引。</param>
        public void CopyTo(TreeingNode<T, TId>[] array, int arrayIndex)
        {
            Nodes.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// 将项插入指定列表的索引处。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        /// <param name="item">给定的项。</param>
        public void Insert(int index, TreeingNode<T, TId> item)
        {
            Nodes.Insert(index, item);
        }

        /// <summary>
        /// 获取指定项所在列表中的索引。
        /// </summary>
        /// <param name="item">指定的树形节点。</param>
        /// <returns>返回整数。</returns>
        public int IndexOf(TreeingNode<T, TId> item)
        {
            return Nodes.IndexOf(item);
        }


        /// <summary>
        /// 移除指定树形节点。
        /// </summary>
        /// <param name="item">给定的树形节点。</param>
        /// <returns>返回布尔值。</returns>
        public bool Remove(TreeingNode<T, TId> item)
        {
            return Nodes.Remove(item);
        }

        /// <summary>
        /// 移除指定索引处的项。
        /// </summary>
        /// <param name="index">给定的索引。</param>
        public void RemoveAt(int index)
        {
            Nodes.RemoveAt(index);
        }

        #endregion

    }

}
