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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.Services
{
    using Core.Services;
    using Requesters;

    /// <summary>
    /// 抓取器服务接口。
    /// </summary>
    public interface ICrawlerService : INetworkService
    {
        /// <summary>
        /// 请求程序管理器。
        /// </summary>
        IServicesManager<IUriRequester> Requesters { get; }

        /// <summary>
        /// 图像文件扩展名集合。
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        string[] ImageExtensions { get; set; }


        /// <summary>
        /// 异步获取链接响应内容中包含的所有图像类超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含图像类超链接列表的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        Task<IList<string>> GetImageLinksAsync(string url, string pattern = null);

        /// <summary>
        /// 异步获取链接响应内容中包含的所有超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含超链接列表的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        Task<IList<string>> GetHyperLinksAsync(string url, string pattern = null);

        /// <summary>
        /// 异步获取链接响应内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="postData">给定用于提交请求的数据（可选；默认不提交数据）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        Task<string> GetContentAsync(string url, string postData = null,
            CancellationToken cancellationToken = default);
    }
}
