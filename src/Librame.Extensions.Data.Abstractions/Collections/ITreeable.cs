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
    /// 可树形接口。
    /// </summary>
    /// <typeparam name="T">指定的树形元素类型。</typeparam>
    public interface ITreeable<T> : ITreeable<T, int>
        where T : IParentId<int>
    {
    }


    /// <summary>
    /// 可树形接口。
    /// </summary>
    /// <typeparam name="T">指定的树形元素类型。</typeparam>
    /// <typeparam name="TId">指定的树形元素标识类型。</typeparam>
    public interface ITreeable<T, TId> : IEnumerable<TreeingNode<T, TId>>
        where T : IParentId<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 节点数。
        /// </summary>
        int Count { get; }
    }
}
