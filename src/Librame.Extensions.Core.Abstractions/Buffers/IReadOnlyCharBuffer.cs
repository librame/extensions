#region License

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
    /// 只读字符缓冲区接口。
    /// </summary>
    public interface IReadOnlyCharBuffer : IReadOnlyBuffer<char>
    {
        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyCharBuffer"/>。</returns>
        new IReadOnlyCharBuffer Copy();
    }
}
