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

namespace Librame.Extensions
{
    /// <summary>
    /// URI 静态扩展。
    /// </summary>
    public static class UriExtensions
    {
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

    }
}
