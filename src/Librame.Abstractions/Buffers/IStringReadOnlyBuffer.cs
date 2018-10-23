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
    /// 字符串只读缓冲区接口。
    /// </summary>
    public interface IStringReadOnlyBuffer : IReadOnlyBuffer<char>
    {
        /// <summary>
        /// 原始字符串。
        /// </summary>
        string RawString { get; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IStringReadOnlyBuffer"/>。</returns>
        new IStringReadOnlyBuffer Copy();
    }
}
