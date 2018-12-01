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

namespace System.Collections.Generic
{
    /// <summary>
    /// 树状列表接口。
    /// </summary>
    /// <typeparam name="T">指定的树状元素类型。</typeparam>
    /// <typeparam name="TId">指定的树状元素标识类型。</typeparam>
    public interface ITreeingList<T, TId> : IEnumerable<TreeingNode<T, TId>>, IList<TreeingNode<T, TId>>
        where T : IParentId<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 节点列表。
        /// </summary>
        IList<TreeingNode<T, TId>> Nodes { get; }
        
        /// <summary>
        /// 无层级节点列表。
        /// </summary>
        IList<TreeingNode<T, TId>> NonstratifiedNodes { get; }
    }
}
