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
using System.Web;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="HttpRequest"/> 实用工具。
    /// </summary>
    public static class HttpRequestUtility
    {
        /// <summary>
        /// 表现为根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRootUrlString(this HttpRequest request)
        {
            return new HttpRequestWrapper(request).AsRootUrlString();
        }
        /// <summary>
        /// 表现为根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRootUrlString(this HttpRequestBase request)
        {
            return string.Format("{0}://{1}", request.Url.Scheme, request.Headers["Host"]);
        }


        /// <summary>
        /// 表现为应用根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/ApplicationPath</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsAppRootUrlString(this HttpRequest request)
        {
            return new HttpRequestWrapper(request).AsAppRootUrlString();
        }
        /// <summary>
        /// 表现为应用根 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/ApplicationPath</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsAppRootUrlString(this HttpRequestBase request)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Headers["Host"],
                request.ApplicationPath == "/" ? string.Empty : request.ApplicationPath);
        }


        /// <summary>
        /// 表现为 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/RawUrl</example>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsUrlString(this HttpRequest request)
        {
            return new HttpRequestWrapper(request).AsUrlString();
        }
        /// <summary>
        /// 表现为 URL 字符串。
        /// </summary>
        /// <example>http://localhost:3030/RawUrl</example>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsUrlString(this HttpRequestBase request)
        {
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Headers["Host"], request.RawUrl);
        }


        /// <summary>
        /// 表现为真实 IP 地址。
        /// </summary>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsIpAddress(this HttpRequest request)
        {
            return new HttpRequestWrapper(request).AsIpAddress();
        }
        /// <summary>
        /// 表现为真实 IP 地址。
        /// </summary>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsIpAddress(this HttpRequestBase request)
        {
            //HTTP_X_FORWARDED_FOR
            string ipAddress = request.ServerVariables["x-forwarded-for"];

            if (!IsEffectiveIpAddress(ipAddress))
                ipAddress = request.ServerVariables["Proxy-Client-IP"];

            if (!IsEffectiveIpAddress(ipAddress))
                ipAddress = request.ServerVariables["WL-Proxy-Client-IP"];

            if (!IsEffectiveIpAddress(ipAddress))
            {
                ipAddress = request.ServerVariables["Remote_Addr"];
                if (ipAddress.Equals("127.0.0.1") || ipAddress.Equals("::1"))
                {
                    // 根据网卡取本机配置的IP
                    var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                    foreach (var address in addressList)
                    {
                        if (address.AddressFamily.ToString() == "InterNetwork")
                        {
                            ipAddress = address.ToString();
                            break;
                        }
                    }
                }
            }

            // 对于通过多个代理的情况，第一个IP为客户端真实IP，多个IP按照英文逗号分割
            if (ipAddress != null && ipAddress.Length > 15)
            {
                if (ipAddress.IndexOf(",") > 0)
                    ipAddress = ipAddress.Substring(0, ipAddress.IndexOf(","));
            }

            return ipAddress;
        }


        /// <summary>
        /// 是否为有效的 IP 地址。
        /// </summary>
        /// <param name="ipAddress">给定的 IP 地址。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEffectiveIpAddress(this string ipAddress)
        {
            return !(string.IsNullOrEmpty(ipAddress)
                || "unknown".Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
        }


        /// <summary>
        /// 是否为本地虚拟路径。
        /// </summary>
        /// <param name="request">给定的 <see cref="HttpRequest"/>。</param>
        /// <param name="url">给定的 URL 字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalUrl(this HttpRequest request, string url)
        {
            return new HttpRequestWrapper(request).IsLocalUrl(url);
        }
        /// <summary>
        /// 是否为本地虚拟路径。
        /// </summary>
        /// <param name="request">给定的 <see cref="HttpRequestBase"/>。</param>
        /// <param name="url">给定的 URL 字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalUrl(this HttpRequestBase request, string url)
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
}
