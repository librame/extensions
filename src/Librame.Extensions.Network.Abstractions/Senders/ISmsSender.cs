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
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 短信服务发送器接口。
    /// </summary>
    public interface ISmsSender : INetworkService
    {
        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="message">给定要发送的消息。</param>
        /// <param name="gatewayUrlFactory">给定的短信网关选项设定工厂方法。</param>
        /// <returns>返回响应的字符串。</returns>
        Task<string> SendAsync(string message, Func<string, Uri> gatewayUrlFactory);

        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="message">给定要发送的消息。</param>
        /// <param name="gatewayUrl">给定的短信网关 URL（可选；默认为选项设定）。</param>
        /// <returns>返回响应的字符串。</returns>
        Task<string> SendAsync(string message, string gatewayUrl = null);
    }
}
