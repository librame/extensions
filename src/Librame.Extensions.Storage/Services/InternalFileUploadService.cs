#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 内部文件上传服务。
    /// </summary>
    internal class InternalFileUploadService : AbstractService<InternalFileUploadService>, IFileUploadService
    {
        private readonly IStorageTokenService _accessToken;


        /// <summary>
        /// 构造一个 <see cref="InternalFileUploadService"/> 实例。
        /// </summary>
        /// <param name="accessToken">给定的 <see cref="IStorageTokenService"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalFileUploadService}"/>。</param>
        public InternalFileUploadService(IStorageTokenService accessToken,
            ILogger<InternalFileUploadService> logger)
            : base(logger)
        {
            _accessToken = accessToken.NotNull(nameof(accessToken));
        }


        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUri">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="configureRequestHeaders">配置请求头部集合的动作（可选）。</param>
        /// <param name="configureContentHeaders">配置请求内容集合的动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public async Task<string> UploadFileAsync(string uploadUri, string filePath,
            Action<HttpRequestHeaders> configureRequestHeaders = null,
            Action<HttpContentHeaders> configureContentHeaders = null,
            CancellationToken cancellationToken = default)
        {
            var results = await UploadFilesAsync(uploadUri,
                new string[] { filePath },
                configureRequestHeaders,
                configureContentHeaders,
                cancellationToken);

            return results.FirstOrDefault();
        }

        /// <summary>
        /// 异步上传文件集合。
        /// </summary>
        /// <param name="uploadUri">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="configureRequestHeaders">配置请求头部集合的动作（可选）。</param>
        /// <param name="configureContentHeaders">配置请求内容集合的动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public async Task<string[]> UploadFilesAsync(string uploadUri, string[] filePaths,
            Action<HttpRequestHeaders> configureRequestHeaders = null,
            Action<HttpContentHeaders> configureContentHeaders = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var results = new List<string>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var token = await _accessToken.GetTokenAsync(default);

                    httpClient.DefaultRequestHeaders.Add("access_token", token);
                    configureRequestHeaders?.Invoke(httpClient.DefaultRequestHeaders);

                    foreach (var file in filePaths)
                    {
                        var buffer = File.ReadAllBytes(file);
                        var content = new ByteArrayContent(buffer);
                        content.Headers.Add("file_extension", Path.GetExtension(file));
                        configureContentHeaders?.Invoke(content.Headers);

                        var response = await httpClient.PostAsync(uploadUri, content);
                        var result = await response.Content.ReadAsStringAsync();
                        results.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
                results.Add("error");
            }
            
            return results.ToArray();
        }

    }
}
