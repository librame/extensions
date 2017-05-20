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

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Uri"/> 实用工具。
    /// </summary>
    public static class UriUtility
    {
        /// <summary>
        /// 是否为 URL 格式。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUrl(this string path)
        {
            path.NotEmpty(nameof(path));

            // 如果包含协议界定符
            return path.Contains(Uri.SchemeDelimiter);
        }

        /// <summary>
        /// 是否为 HTTP 或 HTTPS URL 格式。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsHttpOrHttpsUrl(this string path)
        {
            path.NotEmpty(nameof(path));

            // 如果不是 HTTP 协议
            if (!path.StartsWith(Uri.UriSchemeHttp + Uri.SchemeDelimiter))
            {
                // 验证 HTTPS 协议
                return path.StartsWith(Uri.UriSchemeHttps + Uri.SchemeDelimiter);
            }
            
            return true;
        }


        /// <summary>
        /// 获取内容长度。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="requestFactory">给定的请求动作（可选）。</param>
        /// <returns>返回整数。</returns>
        public static long ReadContentLength(this string url, Action<HttpWebRequest> requestFactory = null)
        {
            var us = new UriStream(url);
            return us.GetContentLength(requestFactory);
        }

        /// <summary>
        /// 读取指定 URL 链接的内容。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="requestFactory">给定的请求动作（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string ReadContent(this string url, Action<HttpWebRequest> requestFactory = null)
        {
            var us = new UriStream(url);
            return us.GetContent(requestFactory);
        }

    }
}
