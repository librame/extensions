#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Encryption.Buffers
{
    /// <summary>
    /// 明文缓冲区接口。
    /// </summary>
    public interface IPlaintextBuffer : IAlgorithmBuffer
    {
        /// <summary>
        /// 明文来源。
        /// </summary>
        string Source { get; }

        /// <summary>
        /// 字符编码。
        /// </summary>
        Encoding Encoding { get; }
    }
}
