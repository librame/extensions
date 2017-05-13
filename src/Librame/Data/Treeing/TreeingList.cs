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

namespace System.Collections.Generic
{
    /// <summary>
    /// 用于泛类型的可树形化对象。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    /// <author>Librame Pang</author>
    public class TreeingList<T, TId> : ITreeingable<T, TId>, IEnumerable<TreeingNode<T, TId>>
        where T : IParentIdDescriptor<TId>
        where TId : struct
    {
        /// <summary>
        /// 获取节点列表。
        /// </summary>
        public IList<TreeingNode<T, TId>> Nodes { get; }


        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/> 实例。
        /// </summary>
        /// <param name="list">给定的类型实例列表。</param>
        public TreeingList(IList<T> list)
        {
            list.NotNull(nameof(list));

            Nodes = ToNodes(list);
        }
        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/> 实例。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        public TreeingList(IList<TreeingNode<T, TId>> nodes)
        {
            Nodes = nodes.NotNull(nameof(nodes));
        }

        /// <summary>
        /// 转换为树形节点列表。
        /// </summary>
        /// <param name="list">给定的对象列表。</param>
        /// <returns>返回树形节点列表。</returns>
        protected virtual IList<TreeingNode<T, TId>> ToNodes(IList<T> list)
        {
            return TreeingHelper.ToNodes<T, TId>(list);
        }


        #region IEnumerable<T> Members

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>返回枚举数。</returns>
        public IEnumerator<TreeingNode<T, TId>> GetEnumerator()
        {
            if (ReferenceEquals(Nodes, null) || Nodes.Count < 1)
            {
                return null;
            }

            return Nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }

}
