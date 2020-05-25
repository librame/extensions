#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Librame.Extensions.Data.Collections
{
    using Core.Identifiers;

    /// <summary>
    /// 树形列表。
    /// </summary>
    /// <typeparam name="T">指定的树形元素类型。</typeparam>
    /// <typeparam name="TId">指定的树形元素标识类型。</typeparam>
    [NotMapped]
    public class TreeingCollection<T, TId> : ITreeable<T, TId>, IEnumerable<TreeingNode<T, TId>>
        where T : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        private readonly IEnumerable<TreeingNode<T, TId>> _nodes;


        /// <summary>
        /// 构造一个 <see cref="TreeingCollection{T, TId}"/>。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        public TreeingCollection(IEnumerable<T> items)
        {
            _nodes = items.AsTreeingNodes<T, TId>();
        }

        /// <summary>
        /// 构造一个 <see cref="TreeingCollection{T, TId}"/>。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        public TreeingCollection(IEnumerable<TreeingNode<T, TId>> nodes)
        {
            _nodes = nodes;
        }



        /// <summary>
        /// 节点数。
        /// </summary>
        public int Count => _nodes.Count();


        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>返回枚举数。</returns>
        public IEnumerator<TreeingNode<T, TId>> GetEnumerator()
            => _nodes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }
}
