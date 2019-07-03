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
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 内部文件传输服务。
    /// </summary>
    internal class InternalFileTransferService : AbstractStorageService, IFileTransferService
    {
        //private static HttpClient _httpClient;
        private readonly IFilePermissionService _tokenService;


        /// <summary>
        /// 构造一个 <see cref="InternalFileTransferService"/> 实例。
        /// </summary>
        /// <param name="tokenService">给定的 <see cref="IFilePermissionService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalFileTransferService(IFilePermissionService tokenService,
            IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _tokenService = tokenService.NotNull(nameof(tokenService));
            Encoding = Options.Encoding;
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding { get; set; }


        /// <summary>
        /// Web 请求头部集合动作。
        /// </summary>
        public Action<WebHeaderCollection> RequestHeadersAction { get; set; }

        public Action<HttpRequestHeaders> HttpRequestHeadersAction { get; set; }


        public async Task<string> DownloadFilesAsync(string uploadUri, string[] filePaths,
            Action<HttpRequestHeaders> configureRequestHeaders = null,
            Action<HttpContentHeaders> configureContentHeaders = null,
            CancellationToken cancellationToken = default)
        {

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
            if (httpClient.IsNull())
            {
                var _httpClient = new HttpClient();

                var token = await _tokenService.GetAccessTokenAsync(cancellationToken);
                _httpClient.DefaultRequestHeaders.Add("access_token", token);

                configureRequestHeaders?.Invoke(_httpClient.DefaultRequestHeaders);
            }

            var results = new List<string>();

            try
            {
                var request = WebRequest.CreateHttp(uploadUri);
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                //request.Referer = "https://www.xxx.com";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
                //request.Host = "www.xxx.com";
                request.Headers.Add("Cookie", cookieStr);

                request.Headers.Add(HttpRequestHeader.Authorization, "");

                foreach (var file in filePaths)
                {
                    var buffer = File.ReadAllBytes(file);
                    //var content = new ByteArrayContent(buffer);
                    //content.Headers.Add("file_extension", Path.GetExtension(file));
                    //configureContentHeaders?.Invoke(content.Headers);

                    //var response = await _httpClient.PostAsync(uploadUri, content, cancellationToken);
                    //var result = await response.Content.ReadAsStringAsync();

                    var requestStream = request.GetRequestStream();
                    await requestStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();

                    var reader = new StreamReader(responseStream, Encoding);
                    var result = await reader.ReadToEndAsync();
                    results.Add(result);
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
