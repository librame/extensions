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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Collections
{
    using Core.Identifiers;

    /// <summary>
    /// 可树形接口。
    /// </summary>
    /// <typeparam name="T">指定实现 <see cref="IParentIdentifier{TId}"/> 的元素类型。</typeparam>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface ITreeable<T, TId> : IEnumerable<TreeingNode<T, TId>>
        where T : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 节点数。
        /// </summary>
        int Count { get; }
    }
}
