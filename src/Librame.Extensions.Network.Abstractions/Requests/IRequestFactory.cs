#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 请求工厂接口。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    public interface IRequestFactory<TRequest> : IEncoding
        where TRequest : class
    {
        /// <summary>
        /// 创建请求。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="method">给定的请求方法（可选；默认 POST）。</param>
        /// <returns>返回 <typeparamref name="TRequest"/>。</returns>
        TRequest CreateRequest(string url, string method = "POST");
    }
}
