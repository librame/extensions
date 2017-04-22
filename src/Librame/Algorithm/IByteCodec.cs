#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Algorithm
{
    /// <summary>
    /// 字节编解码器接口。
    /// </summary>
    public interface IByteCodec
    {
        /// <summary>
        /// 获取 <see cref="Algorithm.AlgorithmSettings"/>。
        /// </summary>
        AlgorithmSettings AlgoSettings { get; }
    }
}
