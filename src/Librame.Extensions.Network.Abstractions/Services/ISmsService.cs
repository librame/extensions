#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading.Tasks;

namespace Librame.Extensions.Network.Services
{
    /// <summary>
    /// 短信服务接口。
    /// </summary>
    public interface ISmsService : INetworkService
    {
        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="mobile">给定的手机。</param>
        /// <param name="text">给定的文本。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        Task<string> SendAsync(string mobile, string text);

        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="descriptors">给定的 <see cref="SmsDescriptor"/> 数组。</param>
        /// <returns>返回一个包含响应内容数组的异步操作。</returns>
        Task<string[]> SendAsync(params SmsDescriptor[] descriptors);
    }
}
