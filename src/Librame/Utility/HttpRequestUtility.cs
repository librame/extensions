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
using System.Web;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="HttpRequest"/> 实用工具。
    /// </summary>
    public class HttpRequestUtility
    {
        /// <summary>
        /// 表现为根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRootUrlString(HttpRequestBase request)
        {
            return string.Format("{0}://{1}", request.Url.Scheme, request.Headers["Host"]);
        }
        /// <summary>
        /// 表现为根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRootUrlString(HttpRequest request)
        {
            return string.Format("{0}://{1}", request.Url.Scheme, request.Headers["Host"]);
        }


        /// <summary>
        /// 表现为应用根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/ApplicationPath</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsApplicationRootUrlString(HttpRequestBase request)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Headers["Host"],
                request.ApplicationPath == "/" ? string.Empty : request.ApplicationPath);
        }
        /// <summary>
        /// 表现为应用根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/ApplicationPath</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsApplicationRootUrlString(HttpRequest request)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Headers["Host"],
                request.ApplicationPath == "/" ? string.Empty : request.ApplicationPath);
        }


        /// <summary>
        /// 表现为 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/RawUrl</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsUrlString(HttpRequestBase request)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Headers["Host"], request.RawUrl);
        }
        /// <summary>
        /// 表现为 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/RawUrl</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsUrlString(HttpRequest request)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Headers["Host"], request.RawUrl);
        }


        /// <summary>
        /// 是否为本地虚拟路径。
        /// </summary>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <param name="url">给定的 URL 字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalUrl(HttpRequestBase request, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            if (url.StartsWith("~/"))
                return true;

            if (url.StartsWith("//") || url.StartsWith("/\\"))
                return false;

            // at this point is the url starts with "/" it is local
            if (url.StartsWith("/"))
                return true;

            // at this point, check for an fully qualified url
            try
            {
                var uri = new Uri(url);
                if (uri.Authority.Equals(request.Headers["Host"], StringComparison.OrdinalIgnoreCase))
                    return true;

                //// finally, check the base url from the settings
                //var workContext = request.RequestContext.GetWorkContext();
                //if (workContext != null) {
                //    var baseUrl = workContext.CurrentSite.BaseUrl;
                //    if (!string.IsNullOrWhiteSpace(baseUrl)) {
                //        if (uri.Authority.Equals(new Uri(baseUrl).Authority, StringComparison.OrdinalIgnoreCase)) {
                //            return true;
                //        }
                //    }
                //}

                return false;
            }
            catch
            {
                // mall-formed url e.g, "abcdef"
                return false;
            }
        }

    }


    /// <summary>
    /// <see cref="HttpRequestUtility"/> 静态扩展。
    /// </summary>
    public static class HttpRequestUtilityExtensions
    {
        /// <summary>
        /// 表现为根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRootUrlString(this HttpRequestBase request)
        {
            return HttpRequestUtility.AsRootUrlString(request);
        }
        /// <summary>
        /// 表现为根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRootUrlString(this HttpRequest request)
        {
            return HttpRequestUtility.AsRootUrlString(request);
        }


        /// <summary>
        /// 表现为应用根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/ApplicationPath</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsApplicationRootUrlString(this HttpRequestBase request)
        {
            return HttpRequestUtility.AsApplicationRootUrlString(request);
        }
        /// <summary>
        /// 表现为应用根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/ApplicationPath</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsApplicationRootUrlString(this HttpRequest request)
        {
            return HttpRequestUtility.AsApplicationRootUrlString(request);
        }


        /// <summary>
        /// 表现为 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/RawUrl</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsUrlString(this HttpRequestBase request)
        {
            return HttpRequestUtility.AsUrlString(request);
        }
        /// <summary>
        /// 表现为 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/RawUrl</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsUrlString(this HttpRequest request)
        {
            return HttpRequestUtility.AsUrlString(request);
        }


        /// <summary>
        /// 是否为本地虚拟路径。
        /// </summary>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <param name="url">给定的 URL 字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalUrl(this HttpRequestBase request, string url)
        {
            return HttpRequestUtility.IsLocalUrl(request, url);
        }

    }
}
