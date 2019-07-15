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
    /// 标识符接口。
    /// </summary>
    public interface IIdentifier
    {
        /// <summary>
        /// 只读存储器。
        /// </summary>
        ReadOnlyMemory<byte> Memory { get; }

        /// <summary>
        /// 转换器。
        /// </summary>
        IIdentifierConverter Converter { get; set; }
    }
}
