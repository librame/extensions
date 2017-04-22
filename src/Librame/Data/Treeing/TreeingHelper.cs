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
        /// <param name="list">给定的对象列表。</param>
        /// <returns>返回树形节点列表。</returns>
        public static IList<TreeingNode<T, TId>> ToNodes<T, TId>(IList<T> list)
            where T : IParentIdDescriptor<TId>
            where TId : struct
        {
            var nodes = new List<TreeingNode<T, TId>>();

            // 提取所有不重复的编号集合
            var ids = list.Select(s => s.Id).Distinct();
            foreach (var id in ids)
            {
                // 查找当前列表项
                var item = list.FirstOrDefault(p => p.Id.Equals(id));

                // 查找所有子列表
                var itemList = list.Where(p => p.ParentId.Equals(id)).ToList();

                if (!ReferenceEquals(itemList, null) && itemList.Count > 0)
                {
                    // 将子列表转换为节点列表
                    var childs = itemList.Select(s => new TreeingNode<T, TId>(s)).ToList();
                    nodes.Add(new TreeingNode<T, TId>(item, childs));
                }
            }

            return nodes;
        }

    }
}
