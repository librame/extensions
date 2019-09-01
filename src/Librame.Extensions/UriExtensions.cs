﻿#region License

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
using System.Net.Sockets;

namespace Librame.Extensions
{
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
        public static bool IsAbsoluteVirtualPath(this string path)
        {
            path.NotNullOrEmpty(nameof(path));

            return path.StartsWith("~/") || path.StartsWith("/");
        }


        /// <summary>
        /// 表示为绝对 URI。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Invalid uri string.
        /// </exception>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public static Uri AsAbsoluteUri(this string absoluteUriString)
        {
            if (!absoluteUriString.IsAbsoluteUri(out Uri result))
                throw new ArgumentException($"Invalid uri string: {absoluteUriString}.");

            return result;
        }


        /// <summary>
        /// 是绝对 URI。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAbsoluteUri(this string absoluteUriString)
        {
            return absoluteUriString.IsAbsoluteUri(out _);
        }
        /// <summary>
        /// 是绝对 URI。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <param name="result">输出 <see cref="Uri"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAbsoluteUri(this string absoluteUriString, out Uri result)
        {
            return Uri.TryCreate(absoluteUriString, UriKind.Absolute, out result);
        }


        /// <summary>
        /// 同一 DNS 主机名或 IP 地址及端口号。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URL 字符串。</param>
        /// <param name="host">给定的 DNS 主机名或 IP 地址及端口号（如：localhost:80）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool SameHost(this string absoluteUriString, string host)
        {
            if (absoluteUriString.IsAbsoluteUri(out Uri result))
                return result.SameHost(host);

            return false;
        }
        /// <summary>
        /// 同一 DNS 主机名或 IP 地址及端口号。
        /// </summary>
        /// <param name="absoluteUri">给定的 <see cref="Uri"/>。</param>
        /// <param name="host">给定的 DNS 主机名或 IP 地址及端口号（如：localhost:80）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool SameHost(this Uri absoluteUri, string host)
        {
            absoluteUri.NotNull(nameof(absoluteUri));

            return absoluteUri.Authority.Equals(host, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 获取 URI 字符串中的主机。
        /// </summary>
        /// <param name="uriString">给定的 URI 字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string GetHost(this string uriString)
        {
            return uriString.GetHost(out _);
        }
        /// <summary>
        /// 获取 URI 字符串中的主机。
        /// </summary>
        /// <param name="uriString">给定的 URI 字符串。</param>
        /// <param name="result">输出可能存在的 <see cref="Uri"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetHost(this string uriString, out Uri result)
        {
            if (uriString.IsAbsoluteUri(out result))
                return result.Authority;

            return uriString;
        }


        /// <summary>
        /// 获取指定路径或 URI 中的路径。
        /// </summary>
        /// <param name="pathOrUri">给定的路径或 URI。</param>
        /// <returns>返回字符串。</returns>
        public static string GetPath(this string pathOrUri)
        {
            return pathOrUri.GetPath(out _);
        }
        /// <summary>
        /// 获取指定路径或 URI 中的路径。
        /// </summary>
        /// <param name="pathOrUri">给定的路径或 URI。</param>
        /// <param name="result">输出可能存在的 <see cref="Uri"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetPath(this string pathOrUri, out Uri result)
        {
            if (pathOrUri.IsAbsoluteUri(out result))
                return result.AbsolutePath;

            if (pathOrUri.StartsWith("~/"))
                return pathOrUri.TrimStart('~');

            return pathOrUri;
        }


        /// <summary>
        /// 获取 URI 字符串中的查询。
        /// </summary>
        /// <param name="uriString">给定的 URI 字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string GetQuery(this string uriString)
        {
            return uriString.GetQuery(out _);
        }
        /// <summary>
        /// 获取 URI 字符串中的查询。
        /// </summary>
        /// <param name="uriString">给定的 URI 字符串。</param>
        /// <param name="result">输出可能存在的 <see cref="Uri"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetQuery(this string uriString, out Uri result)
        {
            if (uriString.IsAbsoluteUri(out result))
                return result.Query;

            return uriString;
        }


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
        public static string CombineUriToString(this string baseUriString, string relativeUri)
        {
            return baseUriString.CombineUri(relativeUri).ToString();
        }
        /// <summary>
        /// 合并 URI 字符串。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineUriToString(this Uri baseUri, string relativeUri)
        {
            return baseUri.CombineUri(relativeUri).ToString();
        }


        /// <summary>
        /// 合并 URI。
        /// </summary>
        /// <param name="baseUriString">给定的基础 URI 字符串。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public static Uri CombineUri(this string baseUriString, string relativeUri)
        {
            return new Uri(baseUriString).CombineUri(relativeUri);
        }
        /// <summary>
        /// 合并 URI。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI。</param>
        /// <param name="relativeUri">给定的相对 URI。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public static Uri CombineUri(this Uri baseUri, string relativeUri)
        {
            return new Uri(baseUri, relativeUri);
        }

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
        {
            return ip.IsIPv6(out IPAddress address) && IPAddress.IPv6Loopback.Equals(address);
        }

        /// <summary>
        /// 是本机 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalIPv4(this string ip)
        {
            return ip.IsIPv4(out IPAddress address) && IPAddress.Loopback.Equals(address);
        }

        /// <summary>
        /// 是本机 IP 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLocalIPAddress(this string ip)
        {
            return ip.IsIPAddress(out IPAddress address) && IPAddress.IsLoopback(address);
        }


        /// <summary>
        /// 是 IPv6 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv6(this string ip)
        {
            return ip.IsIPv6(out _);
        }

        /// <summary>
        /// 是 IPv6 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <param name="address">输出 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv6(this string ip, out IPAddress address)
        {
            return ip.IsIPAddress(out address) && address.AddressFamily == AddressFamily.InterNetworkV6;
        }


        /// <summary>
        /// 是 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(this string ip)
        {
            return ip.IsIPv4(out _);
        }

        /// <summary>
        /// 是 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <param name="address">输出 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(this string ip, out IPAddress address)
        {
            return ip.IsIPAddress(out address) && address.AddressFamily == AddressFamily.InterNetwork;
        }


        /// <summary>
        /// 是 IP 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPAddress(this string ip)
        {
            return ip.IsIPAddress(out _);
        }

        /// <summary>
        /// 是 IP 地址。
        /// </summary>
        /// <param name="ip">给定的 IP。</param>
        /// <param name="address">输出 <see cref="IPAddress"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPAddress(this string ip, out IPAddress address)
        {
            return IPAddress.TryParse(ip, out address);
        }

        #endregion

    }
}