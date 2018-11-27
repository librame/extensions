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
    /// 字符缓冲区接口。
    /// </summary>
    public interface ICharBuffer : IBuffer<char>, IReadOnlyCharBuffer
    {
        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        new ICharBuffer Copy();


        /// <summary>
        /// 获取字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string GetString();
    }
}
