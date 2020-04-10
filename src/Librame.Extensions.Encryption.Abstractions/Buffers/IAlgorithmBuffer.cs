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

namespace Librame.Extensions.Encryption.Buffers
{
    /// <summary>
    /// 算法缓冲区接口。
    /// </summary>
    public interface IAlgorithmBuffer : IEquatable<IAlgorithmBuffer>
    {
        /// <summary>
        /// 服务提供程序。
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 当前缓冲区。
        /// </summary>
        ReadOnlyMemory<byte> CurrentBuffer { get; }


        /// <summary>
        /// 改变缓冲区。
        /// </summary>
        /// <param name="newBufferFactory">给定的新缓冲区工厂方法。</param>
        /// <returns>返回 <see cref="IAlgorithmBuffer"/>。</returns>
        IAlgorithmBuffer ChangeBuffer(Func<byte[], byte[]> newBufferFactory);

        /// <summary>
        /// 改变缓冲区。
        /// </summary>
        /// <param name="newBuffer">给定的新缓冲区。</param>
        /// <returns>返回 <see cref="IAlgorithmBuffer"/>。</returns>
        IAlgorithmBuffer ChangeBuffer(byte[] newBuffer);
    }
}
