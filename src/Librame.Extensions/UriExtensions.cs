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
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace Librame.Extensions
{
    using Resources;

    /// <summary>
    /// URI 静态扩展。
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// 是否为绝对虚拟路径。
        /// </summary>
        /// <example>
        /// ~/VirtualPath or /VirtualPath.
        /// </example>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsAbsoluteVirtualPath(this string path)
        {
            path.NotEmpty(nameof(path));
            return path.StartsWith("~/", StringComparison.OrdinalIgnoreCase) || path.StartsWith("/", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 是绝对 URI。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static bool IsAbsoluteUri(this string absoluteUriString)
            => absoluteUriString.IsAbsoluteUri(out _);

        /// <summary>
        /// 是绝对 URI。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <param name="result">输出 <see cref="Uri"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static bool IsAbsoluteUri(this string absoluteUriString, out Uri result)
            => Uri.TryCreate(absoluteUriString, UriKind.Absolute, out result);


        /// <summary>
        /// 表示为绝对 URI。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Invalid uri string.
        /// </exception>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static Uri AsAbsoluteUri(this string absoluteUriString)
        {
            if (!absoluteUriString.IsAbsoluteUri(out Uri result))
                throw new ArgumentException(InternalResource.ArgumentExceptionAbsoluteUriStringFormat.Format(absoluteUriString));

            return result;
        }


        /// <summary>
        /// 同一 DNS 主机名或 IP 地址及端口号。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URL 字符串。</param>
        /// <param name="host">给定的 DNS 主机名或 IP 地址及端口号（如：localhost:8000）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool SameHost(this string absoluteUriString, string host)
            => absoluteUriString.IsAbsoluteUri(out Uri result) ? result.SameHost(host) : false;

        /// <summary>
        /// 同一 DNS 主机名或 IP 地址及端口号。
        /// </summary>
        /// <param name="absoluteUri">给定的 <see cref="Uri"/>。</param>
        /// <param name="host">给定的 DNS 主机名或 IP 地址及端口号（如：localhost:80）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool SameHost(this Uri absoluteUri, string host)
            => absoluteUri.NotNull(nameof(absoluteUri)).Authority.Equals(host, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 获取 URI 字符串中的主机。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static string GetHost(this string absoluteUriString)
            => absoluteUriString.GetHost(out _);

        /// <summary>
        /// 获取 URI 字符串中的主机。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <param name="result">输出可能存在的 <see cref="Uri"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static string GetHost(this string absoluteUriString, out Uri result)
            => absoluteUriString.IsAbsoluteUri(out result) ? result.Authority : absoluteUriString;


        /// <summary>
        /// 获取指定路径或 URI 中的路径。
        /// </summary>
        /// <param name="pathOrUri">给定的路径或 URI。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static string GetPath(this string pathOrUri)
            => pathOrUri.GetPath(out _);

        /// <summary>
        /// 获取指定路径或 URI 中的路径。
        /// </summary>
        /// <param name="pathOrUri">给定的路径或 URI。</param>
        /// <param name="result">输出可能存在的 <see cref="Uri"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string GetPath(this string pathOrUri, out Uri result)
        {
            if (pathOrUri.IsAbsoluteUri(out result))
                return result.AbsolutePath;

            if (pathOrUri.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                return pathOrUri.TrimStart('~');

            return pathOrUri;
        }


        /// <summary>
        /// 获取 URI 字符串中的查询。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static string GetQuery(this string absoluteUriString)
            => absoluteUriString.GetQuery(out _);

        /// <summary>
        /// 获取 URI 字符串中的查询。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <param name="result">输出可能存在的 <see cref="Uri"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static string GetQuery(this string absoluteUriString, out Uri result)
            => absoluteUriString.IsAbsoluteUri(out result) ? result.Query : absoluteUriString;


        #region CombineUri

        /// <summary>
        /// 合并 URI 字符串。
        /// </summary>
        /// <remarks>
        /// 样例1：
        /// <code>
        /// var baseUri = "http://www.domain.name/";
        /// var relativeUri = "controller/action";
        /// // http://www.domain.name/controller/action
        /// return baseUri.CombineUriToString(relativeUri);
        /// </code>
        /// 样例2：
        /// <code>
        /// var baseUri = "http://www.domain.name/webapi/entities";
        /// var relativeUri = "/controller/action";
        /// // http://www.domain.name/controller/action
        /// return baseUri.CombineUriToString(relativeUri);
        /// </code>
        /// </remarks>
        /// <param name="baseUriString">给定的基础 URI 字符串。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings")]
        public static string CombineUriToString(this string baseUriString, string relativeUri)
            => baseUriString.CombineUri(relativeUri).ToString();

        /// <summary>
        /// 合并 URI 字符串。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static string CombineUriToString(this Uri baseUri, string relativeUri)
            => baseUri.CombineUri(relativeUri).ToString();


        /// <summary>
        /// 合并 URI。
        /// </summary>
        /// <param name="baseUriString">给定的基础 URI 字符串。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static Uri CombineUri(this string baseUriString, string relativeUri)
            => new Uri(baseUriString).CombineUri(relativeUri);

        /// <summary>
        /// 合并 URI。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        public static Uri CombineUri(this Uri baseUri, string relativeUri)
            => new Uri(baseUri, relativeUri);

        #endregion


        #region IPAddress

        /// <summary>
        /// 是否为 NULL、IPv4.None 或 IPv6.None。
        /// </summary>
        /// <param name="address">给定的 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullOrNone(this IPAddress address)
        {
            return address.IsNull()
                || IPAddress.None.Equals(address)
                || IPAddress.IPv6None.Equals(address);
        }


        /// <summary>
        /// 是本机 IPv6 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalIPv6(this string ip)
            => ip.IsIPv6(out IPAddress result) && result.IsIPv6Loopback();

        /// <summary>
        /// 是本机 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalIPv4(this string ip)
            => ip.IsIPv4(out IPAddress result) && result.IsIPv4Loopback();

        /// <summary>
        /// 是环回地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLoopbackIPAddress(this string ip)
            => ip.IsIPAddress(out IPAddress result) && result.IsLoopback();

        /// <summary>
        /// 是 IPv6 环回地址。
        /// </summary>
        /// <param name="address">给定的 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv6Loopback(this IPAddress address)
            => IPAddress.IPv6Loopback.Equals(address);

        /// <summary>
        /// 是 IPv4 环回地址。
        /// </summary>
        /// <param name="address">给定的 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4Loopback(this IPAddress address)
            => IPAddress.Loopback.Equals(address);

        /// <summary>
        /// 是环回地址。
        /// </summary>
        /// <param name="address">给定的 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLoopback(this IPAddress address)
            => IPAddress.IsLoopback(address);


        /// <summary>
        /// 是 IPv6 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv6(this string ip)
            => ip.IsIPv6(out _);

        /// <summary>
        /// 是 IPv6 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <param name="result">输出 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv6(this string ip, out IPAddress result)
            => ip.IsIPAddress(out result) && result.IsIPv6();


        /// <summary>
        /// 是 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(this string ip)
            => ip.IsIPv4(out _);

        /// <summary>
        /// 是 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <param name="result">输出 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(this string ip, out IPAddress result)
            => ip.IsIPAddress(out result) && result.IsIPv4();

        /// <summary>
        /// 是 IPv4 地址。
        /// </summary>
        /// <param name="address">给定的 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(this IPAddress address)
            => address?.AddressFamily == AddressFamily.InterNetwork;

        /// <summary>
        /// 是 IPv6 地址。
        /// </summary>
        /// <param name="address">给定的 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv6(this IPAddress address)
            => address?.AddressFamily == AddressFamily.InterNetworkV6;


        /// <summary>
        /// 是 IP 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPAddress(this string ip)
            => ip.IsIPAddress(out _);

        /// <summary>
        /// 是 IP 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <param name="result">输出 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPAddress(this string ip, out IPAddress result)
            => IPAddress.TryParse(ip, out result);

        #endregion

    }
}
