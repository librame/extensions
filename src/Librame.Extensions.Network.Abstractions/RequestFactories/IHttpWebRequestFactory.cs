#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Net;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// HTTP 请求程序接口。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    public interface IHttpRequester<TRequest> : IRequestFactory
        where TRequest : class
    {
        /// <summary>
        /// 创建请求。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="method">给定的请求方法（可选；默认 POST）。</param>
        /// <returns>返回 <see cref="HttpWebRequest"/>。</returns>
        TRequest CreateRequest(string url, string method = "POST");
    }
}
