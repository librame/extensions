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

namespace Librame.Extensions.Core.Identifiers
{
    using Serializers;

    /// <summary>
    /// 算法标识符接口。
    /// </summary>
    public interface IAlgorithmIdentifier : IEquatable<IAlgorithmIdentifier>
    {
        /// <summary>
        /// 只读内存。
        /// </summary>
        SerializableObject<ReadOnlyMemory<byte>> ReadOnlyMemory { get; }
    }
}
