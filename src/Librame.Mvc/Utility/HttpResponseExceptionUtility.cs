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
using System.Net;
using System.Web.Http;

namespace Librame.Utility
{
    /// <summary>
    /// HTTP 响应异常实用工具。
    /// </summary>
    public static class HttpResponseExceptionUtility
    {
        /// <summary>
        /// 无效的 HTTP 请求。
        /// </summary>
        /// <param name="invalid">给定的表示无效的布尔值。</param>
        /// <param name="statusCode">给定的 HTTP 状态码（可选）。</param>
        /// <returns>返回是否无效的布尔值。</returns>
        public static bool InvalidHttpRequest(this bool invalid,
            HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            if (invalid)
                throw new HttpResponseException(statusCode);

            return invalid;
        }

        /// <summary>
        /// 无效的 HTTP 请求（内部已集成是否为空值的验证）。
        /// </summary>
        /// <param name="value">给定的值。</param>
        /// <param name="invalidFactory">用于验证无效的工厂方法（此参数为空表示有效）。</param>
        /// <param name="statusCode">给定的 HTTP 状态码（可选）。</param>
        /// <returns>返回是否无效的布尔值。</returns>
        public static bool InvalidHttpRequest<TValue>(this TValue value, Func<TValue, bool> invalidFactory,
            HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            if (value == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            if (invalidFactory == null)
                return false; // 表示请求有效
            
            if (invalidFactory.Invoke(value))
                throw new HttpResponseException(statusCode);

            return false; // 表示请求有效
        }

    }
}
