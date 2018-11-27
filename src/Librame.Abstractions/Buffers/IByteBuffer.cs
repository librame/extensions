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
    /// 字节缓冲区接口。
    /// </summary>
    public interface IByteBuffer : IBuffer<byte>, IReadOnlyByteBuffer
    {
        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        new IByteBuffer Copy();
    }
}
