#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data.Descriptors;
using Librame.Utility;
using System.Runtime.InteropServices;

namespace System.Collections.Generic
{
    /// <summary>
    /// 用于泛类型的可树形化列表。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class TreeingList<T, TId> : ITreetable<T, TId>, IEnumerable<TreeingNode<T, TId>>
        where T : IParentIdDescriptor<TId>
        where TId : struct
    {
        /// <summary>
        /// 获取节点列表。
        /// </summary>
        public IList<TreeingNode<T, TId>> Nodes { get; }

        /// <summary>
        /// 获取无层级节点列表。
        /// </summary>
        public IList<TreeingNode<T, TId>> NonstratifiedNodes { get; private set; }


        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/> 实例。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        public TreeingList(IEnumerable<T> items)
        {
            items.NotNull(nameof(items));

            Nodes = ToNodes(items);
            NonstratifiedNodes = ToNonstratifiedNodes(Nodes);
        }
        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/> 实例。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        public TreeingList(IList<TreeingNode<T, TId>> nodes)
        {
            Nodes = nodes.NotNull(nameof(nodes));
            NonstratifiedNodes = ToNonstratifiedNodes(nodes);
        }


        /// <summary>
        /// 转换为树形节点列表。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        /// <returns>返回树形节点列表。</returns>
        protected virtual IList<TreeingNode<T, TId>> ToNodes(IEnumerable<T> items)
        {
            return TreeingHelper.ToNodes<T, TId>(items);
        }

        /// <summary>
        /// 转换为无层级树形节点列表。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        /// <returns>返回无层级树形节点列表。</returns>
        protected virtual IList<TreeingNode<T, TId>> ToNonstratifiedNodes(IList<TreeingNode<T, TId>> nodes)
        {
            List<TreeingNode<T, TId>> nonstratifiedNodes = null;

            if (nodes != null && nodes.Count > 0)
            {
                nonstratifiedNodes = new List<TreeingNode<T, TId>>();

                foreach (var n in nodes)
                {
                    nonstratifiedNodes.Add(n);

                    if (n.HasChilds)
                    {
                        // 链式查询
                        var lookupNodes = ToNonstratifiedNodes(n.Childs);
                        nonstratifiedNodes.AddRange(lookupNodes);
                    }
                }
            }

            return nonstratifiedNodes;
        }


        #region IEnumerable<T> Members

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>返回枚举数。</returns>
        public IEnumerator<TreeingNode<T, TId>> GetEnumerator()
        {
            if (Nodes == null || Nodes.Count < 1)
                return null;

            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }

}
