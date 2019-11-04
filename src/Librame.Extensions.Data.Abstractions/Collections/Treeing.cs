#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 树形集合。
    /// </summary>
    /// <typeparam name="T">指定的树形元素类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [NotMapped]
    public class Treeing<T> : Treeing<T, int>, ITreeable<T>
        where T : IParentId<int>
    {
        /// <summary>
        /// 构造一个 <see cref="Treeing{T}"/>。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        public Treeing(IEnumerable<T> items)
            : base(items)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="Treeing{T}"/>。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        public Treeing(IEnumerable<TreeingNode<T, int>> nodes)
            : base(nodes)
        {
        }
    }


    /// <summary>
    /// 树形列表。
    /// </summary>
    /// <typeparam name="T">指定的树形元素类型。</typeparam>
    /// <typeparam name="TId">指定的树形元素标识类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [NotMapped]
    public class Treeing<T, TId> : ITreeable<T, TId>, IEnumerable<TreeingNode<T, TId>>
        where T : IParentId<TId>
        where TId : IEquatable<TId>
    {
        private readonly IEnumerable<TreeingNode<T, TId>> _nodes;


        /// <summary>
        /// 构造一个 <see cref="Treeing{T, TId}"/>。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        public Treeing(IEnumerable<T> items)
        {
            _nodes = items.AsTreeingNodes<T, TId>();
        }
        /// <summary>
        /// 构造一个 <see cref="Treeing{T, TId}"/>。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        public Treeing(IEnumerable<TreeingNode<T, TId>> nodes)
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
