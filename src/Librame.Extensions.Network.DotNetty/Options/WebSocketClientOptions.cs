#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.DotNetty.Options
{
    /// <summary>
    /// WebSocket 客户端。
    /// </summary>
    public class WebSocketClientOptions : ClientOptions
    {
        /// <summary>
        /// 虚拟路径。
        /// </summary>
        public string VirtualPath { get; set; }
            = "/websocket";
    }
}
