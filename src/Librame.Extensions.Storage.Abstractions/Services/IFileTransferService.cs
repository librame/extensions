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
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 文件传输服务接口。
    /// </summary>
    public interface IFileTransferService : IService, IEncoding
    {
        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUri">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="configureRequestHeaders">配置请求头部集合的动作（可选）。</param>
        /// <param name="configureContentHeaders">配置请求内容集合的动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string> UploadFileAsync(string uploadUri, string filePath,
            Action<HttpRequestHeaders> configureRequestHeaders = null,
            Action<HttpContentHeaders> configureContentHeaders = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步上传文件集合。
        /// </summary>
        /// <param name="uploadUri">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="configureRequestHeaders">配置请求头部集合的动作（可选）。</param>
        /// <param name="configureContentHeaders">配置请求内容集合的动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string[]> UploadFilesAsync(string uploadUri, string[] filePaths,
            Action<HttpRequestHeaders> configureRequestHeaders = null,
            Action<HttpContentHeaders> configureContentHeaders = null,
            CancellationToken cancellationToken = default);
    }
}
