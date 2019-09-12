﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 字符型只读的连续内存缓冲区接口。
    /// </summary>
    public interface ICharReadOnlyMemoryBuffer : IReadOnlyMemoryBuffer<char>
    {
    }
}