#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.DotNetty.Demo
{
    /// <summary>
    /// 客户端选项。
    /// </summary>
    public class ClientOptions : RemoteOptions
    {
        /// <summary>
        /// 缓冲区大小。
        /// </summary>
        public int BufferSize { get; set; }
            = 256;

        /// <summary>
        /// 重试次数。
        /// </summary>
        public int RetryCount { get; set; }
            = 3;
    }
}
