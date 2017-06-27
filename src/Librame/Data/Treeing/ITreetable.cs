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

namespace System.Collections.Generic
{
    /// <summary>
    /// 用于泛类型的可树形化接口。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    /// <typeparam name="TId">指定的主键类型。</typeparam>
    /// <author>Librame Pang</author>
    public interface ITreetable<T, TId> : IEnumerable<TreeingNode<T, TId>>
        where T : IParentIdDescriptor<TId>
        where TId : struct
    {
        /// <summary>
        /// 获取节点列表。
        /// </summary>
        IList<TreeingNode<T, TId>> Nodes { get; }
        
        /// <summary>
        /// 获取无层级节点列表。
        /// </summary>
        IList<TreeingNode<T, TId>> NonstratifiedNodes { get; }
    }


    /// <summary>
    /// <see cref="TreeingList{T, TId}"/> 静态扩展。
    /// </summary>
    public static class TreeingListExtensions
    {

        /// <summary>
        /// 转换为树形化集合。
        /// </summary>
        /// <typeparam name="T">列表中元素的类型。</typeparam>
        /// <param name="items">给定的项集合。</param>
        /// <returns>返回树形化接口。</returns>
        public static ITreetable<T, int> AsTreeing<T>(this IEnumerable<T> items)
            where T : IParentIdDescriptor<int>
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
        public static ITreetable<T, TId> AsTreeing<T, TId>(this IEnumerable<T> items)
            where T : IParentIdDescriptor<TId>
            where TId : struct
        {
            return new TreeingList<T, TId>(items);
        }

    }
}
