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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 算法标识符接口。
    /// </summary>
    public interface IAlgorithmIdentifier : IEquatable<IAlgorithmIdentifier>
    {
        /// <summary>
        /// 只读的连续内存区域。
        /// </summary>
        ReadOnlyMemory<byte> Memory { get; }

        /// <summary>
        /// 算法转换器。
        /// </summary>
        IAlgorithmConverter Converter { get; }
    }
}
