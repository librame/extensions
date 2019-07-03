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
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 内部文件传输服务。
    /// </summary>
    internal class InternalFileTransferService : AbstractStorageService, IFileTransferService
    {
        private readonly IFilePermissionService _permissionService;


        /// <summary>
        /// 构造一个 <see cref="InternalFileTransferService"/> 实例。
        /// </summary>
        /// <param name="permissionService">给定的 <see cref="IFilePermissionService"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{StorageBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalFileTransferService(IFilePermissionService permissionService,
            IOptions<CoreBuilderOptions> coreOptions,
            IOptions<StorageBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _permissionService = permissionService.NotNull(nameof(permissionService));
            Encoding = coreOptions.NotNull(nameof(coreOptions)).Value.Encoding;
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 使用访问令牌。
        /// </summary>
        public bool UseAccessToken { get; set; }
            = false;

        /// <summary>
        /// 使用授权码。
        /// </summary>
        public bool UseAuthorizationCode { get; set; }
            = false;

        /// <summary>
        /// 使用 Cookie 值。
        /// </summary>
        public bool UseCookieValue { get; set; }
            = false;


        /// <summary>
        /// 异步下载文件。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public Task<string> DownloadFileAsync(string downloadUrl, string filePath,
            CancellationToken cancellationToken = default)
        {
            return DownloadFilesAsync(downloadUrl, new string[] { filePath },
                cancellationToken);
        }

        /// <summary>
        /// 异步下载文件集合。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public async Task<string> DownloadFilesAsync(string downloadUrl, string[] filePaths,
            CancellationToken cancellationToken = default)
        {
            string response = null;

            try
            {
                var hwr = await CreateRequestAsync(downloadUrl);

            }
            catch (Exception ex)
            {
                response = ex.AsInnerMessage();

                Logger.LogError(ex, response);
            }

            return response;
        }


        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUrl">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public Task<string> UploadFileAsync(string uploadUrl, string filePath,
            CancellationToken cancellationToken = default)
        {
            return UploadFilesAsync(uploadUrl, new string[] { filePath },
                cancellationToken);
        }

        /// <summary>
        /// 异步上传文件集合。
        /// </summary>
        /// <param name="uploadUrl">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public async Task<string> UploadFilesAsync(string uploadUrl, string[] filePaths,
            CancellationToken cancellationToken = default)
        {
            string response = null;

            try
            {
                var hwr = await CreateRequestAsync(uploadUrl);

                foreach (var file in filePaths)
                {
                    var buffer = File.ReadAllBytes(file);
                    //var content = new ByteArrayContent(buffer);
                    //content.Headers.Add("file_extension", Path.GetExtension(file));
                    //configureContentHeaders?.Invoke(content.Headers);

                    //var response = await _httpClient.PostAsync(uploadUri, content, cancellationToken);
                    //var result = await response.Content.ReadAsStringAsync();

                    using (var s = hwr.GetRequestStream())
                    {
                        await s.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    }
                }

                using (var s = hwr.GetResponse().GetResponseStream())
                {
                    if (s.IsNotNull())
                    {
                        using (var sr = new StreamReader(s, Encoding))
                        {
                            response = await sr.ReadToEndAsync();
                            Logger.LogDebug($"Response: {response}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.AsInnerMessage();

                Logger.LogError(ex, response);
            }
            
            return response;
        }

        private async Task<HttpWebRequest> CreateRequestAsync(string url, string method = "POST",
            CancellationToken cancellationToken = default)
        {
            var hwr = WebRequest.CreateHttp(url);
            //hwr.AllowAutoRedirect = opts.AllowAutoRedirect;
            //hwr.Referer = opts.Referer;
            //hwr.Timeout = opts.Timeout;
            hwr.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
            hwr.Method = method;

            if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                hwr.ContentType = $"application/x-www-form-urlencoded; charset={Encoding.AsName()}";

            if (UseAccessToken)
            {
                var accessToken = await _permissionService.GeAccessTokenAsync(cancellationToken);
                hwr.Headers.Add("access_token", accessToken);
            }

            if (UseAuthorizationCode)
            {
                var authorizationCode = await _permissionService.GetAuthorizationCodeAsync(cancellationToken);
                hwr.Headers.Add(HttpRequestHeader.Authorization, authorizationCode);
            }

            if (UseCookieValue)
            {
                var cookieValue = await _permissionService.GetCookieValueAsync(cancellationToken);
                hwr.Headers.Add(HttpRequestHeader.Cookie, cookieValue);
            }

            return hwr;
        }

    }
}
