#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 抓取器服务接口。
    /// </summary>
    public interface ICrawlerService : INetworkService
    {
        /// <summary>
        /// 图像文件扩展名集合。
        /// </summary>
        string[] ImageExtensions { get; set; }


        /// <summary>
        /// 异步获取链接响应内容中包含的所有图像类超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含图像类超链接列表的异步操作。</returns>
        Task<IList<string>> GetImageHyperLinksAsync(string url, string pattern = null);


        /// <summary>
        /// 异步获取链接响应内容中包含的所有超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含超链接列表的异步操作。</returns>
        Task<IList<string>> GetHyperLinksAsync(string url, string pattern = null);


        /// <summary>
        /// 异步获取链接响应的内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        Task<string> GetStringAsync(string url);


        /// <summary>
        /// 异步提交链接响应的内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="postData">给定用于提交请求的数据。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        Task<string> PostStringAsync(string url, string postData);
    }
}
