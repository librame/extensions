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
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 树形助手。
    /// </summary>
    public class TreeingHelper
    {
        /// <summary>
        /// 转换为树形节点列表。
        /// </summary>
        /// <param name="items">给定的类型实例集合。</param>
        /// <returns>返回树形节点列表。</returns>
        public static IList<TreeingNode<T, TId>> ToNodes<T, TId>(IEnumerable<T> items)
            where T : IParentIdDescriptor<TId>
            where TId : struct
        {
            if (items == null) return null;

            var rootParentId = items.Select(s => s.ParentId).Min();

            return LookupNodes(items, rootParentId);
        }

        private static IList<TreeingNode<T, TId>> LookupNodes<T, TId>(IEnumerable<T> items,
            TId parentId, int depthLevel = 0)
            where T : IParentIdDescriptor<TId>
            where TId : struct
        {
            var parents = items.Where(p => p.ParentId.Equals(parentId)).ToList();

            if (parents.Count < 1)
                return null;

            var nodes = new List<TreeingNode<T, TId>>();
            foreach (var p in parents)
            {
                var node = new TreeingNode<T, TId>(p, depthLevel);
                
                var childs = LookupNodes(items, p.Id, node.DepthLevel + 1);
                node.Childs = childs;

                nodes.Add(node);
            }

            return nodes;
        }

    }
}
