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
using System.Linq;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// 树形列表可枚举静态扩展。
    /// </summary>
    public static class TreeingListEnumerableExtensions
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


        /// <summary>
        /// 异步转换为树形节点列表。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        /// <returns>返回一个包含树形节点列表的异步操作。</returns>
        public static Task<IList<TreeingNode<T, TId>>> AsTreeingNodesAsync<T, TId>(this IEnumerable<T> items)
            where T : IParentId<TId>
            where TId : IEquatable<TId>
        {
            return Task.Factory.StartNew(() => items.AsTreeingNodes<T, TId>());
        }

        /// <summary>
        /// 转换为树形节点列表。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        /// <returns>返回树形节点列表。</returns>
        public static IList<TreeingNode<T, TId>> AsTreeingNodes<T, TId>(this IEnumerable<T> items)
            where T : IParentId<TId>
            where TId : IEquatable<TId>
        {
            if (items.IsEmpty()) return null;

            var rootParentId = items.Select(s => s.ParentId).Min();

            return LookupNodes(items, rootParentId);
        }


        private static IList<TreeingNode<T, TId>> LookupNodes<T, TId>(IEnumerable<T> items,
            TId parentId, int depthLevel = 0)
            where T : IParentId<TId>
            where TId : IEquatable<TId>
        {
            var parents = items.Where(p => p.ParentId.Equals(parentId)).ToList();

            if (parents.Count < 1)
                return null;

            var nodes = new List<TreeingNode<T, TId>>();

            foreach (var p in parents)
            {
                var node = new TreeingNode<T, TId>(p, depthLevel);

                var children = LookupNodes(items, p.Id, node.DepthLevel + 1);
                node.Children = children;

                nodes.Add(node);
            }

            return nodes;
        }

    }
}
