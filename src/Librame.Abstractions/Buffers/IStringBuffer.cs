#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Buffers
{
    /// <summary>
    /// 字符串缓冲区接口。
    /// </summary>
    public interface IStringBuffer : IBuffer<char>, IStringReadOnlyBuffer
    {
        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IStringBuffer"/>。</returns>
        new IStringBuffer Copy();
    }
}
