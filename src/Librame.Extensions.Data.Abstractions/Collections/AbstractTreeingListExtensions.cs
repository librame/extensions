#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象树形列表静态扩展。
    /// </summary>
    public static class AbstractTreeingListExtensions
    {
        
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
