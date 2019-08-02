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
    internal class InternalFileTransferService : ExtensionBuilderServiceBase<StorageBuilderOptions>, IFileTransferService
    {
        private readonly IFilePermissionService _permissionService;


        /// <summary>
        /// 构造一个 <see cref="InternalFileTransferService"/> 实例。
        /// </summary>
        /// <param name="permission">给定的 <see cref="IFilePermissionService"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        public InternalFileTransferService(IFilePermissionService permission, IOptions<CoreBuilderOptions> coreOptions)
            : base(permission.CastTo<IFilePermissionService, ExtensionBuilderServiceBase<StorageBuilderOptions>>(nameof(permission)))
        {
            _permissionService = permission;

            Encoding = coreOptions.NotNull(nameof(coreOptions)).Value.Encoding;
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 使用访问令牌（默认禁用）。
        /// </summary>
        public bool UseAccessToken { get; set; }
            = false;

        /// <summary>
        /// 使用授权码（默认禁用）。
        /// </summary>
        public bool UseAuthorizationCode { get; set; }
            = false;

        /// <summary>
        /// 使用 Cookie 值（默认禁用）。
        /// </summary>
        public bool UseCookieValue { get; set; }
            = false;

        /// <summary>
        /// 使用断点续传（默认使用）。
        /// </summary>
        public bool UseBreakpointResume { get; set; }
            = true;

        /// <summary>
        /// 进度动作（传入参数依次为总长度、当前位置）。
        /// </summary>
        public Action<long, long> ProgressAction { get; set; }


        /// <summary>
        /// 异步下载文件。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="IFileLocator"/> 的异步操作。</returns>
        public async Task<IFileLocator> DownloadFileAsync(string downloadUrl, string savePath,
            CancellationToken cancellationToken = default)
        {
            var hwr = await CreateRequestAsync(downloadUrl, "GET", cancellationToken);

            var buffer = new byte[Options.BufferSize];
            var range = 0L;

            if (File.Exists(savePath) && UseBreakpointResume)
            {
                using (var fs = File.OpenRead(savePath))
                {
                    var readCount = 1;
                    while (readCount > 0)
                    {
                        // 每次从流中读取指定缓冲区的字节数，当读完后退出循环
                        readCount = fs.Read(buffer, 0, buffer.Length);
                    }

                    range = fs.Position;
                }

                hwr.AddRange(range);
            }

            using (var wr = hwr.GetResponse())
            {
                // Accept-Ranges: bytes or none.
                var acceptRanges = wr.Headers[HttpResponseHeader.AcceptRanges];
                var supportRanges = acceptRanges?.Contains("bytes");

                using (var s = wr.GetResponseStream())
                {
                    var writeMode = FileMode.Create;

                    // 如果需要且服务端支持 Ranges，才启用续传
                    if (range > 0 && supportRanges.Value && s.CanSeek)
                    {
                        s.Seek(range, SeekOrigin.Begin);
                        writeMode = FileMode.Append;
                    }

                    using (var fs = File.Open(savePath, writeMode))
                    {
                        var readCount = 1;
                        while (readCount > 0)
                        {
                            // 每次从流中读取指定缓冲区的字节数，当读完后退出循环
                            readCount = s.Read(buffer, 0, buffer.Length);

                            // 将读取到的缓冲区字节数写入文件流
                            fs.Write(buffer, 0, readCount);

                            ProgressAction?.Invoke(s.Length, fs.Position);
                        }
                    }
                }
            }

            return savePath.AsFileLocator();
        }


        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUrl">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        public async Task<string> UploadFileAsync(string uploadUrl, string filePath,
            CancellationToken cancellationToken = default)
        {
            string response = null;

            var hwr = await CreateRequestAsync(uploadUrl, cancellationToken: cancellationToken);

            using (var s = hwr.GetRequestStream())
            {
                var buffer = new byte[Options.BufferSize];

                using (var fs = File.OpenRead(filePath))
                {
                    var readCount = 1;
                    while (readCount > 0)
                    {
                        // 每次从文件流中读取指定缓冲区的字节数，当读完后退出循环
                        readCount = fs.Read(buffer, 0, buffer.Length);

                        // 将读取到的缓冲区字节数写入请求流
                        s.Write(buffer, 0, readCount);

                        ProgressAction?.Invoke(fs.Length, s.Position);
                    }
                }
            }

            using (var s = hwr.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s, Encoding))
                {
                    response = await sr.ReadToEndAsync();
                    Logger.LogDebug($"Response: {response}");
                }
            }

            return response;
        }


        private async Task<HttpWebRequest> CreateRequestAsync(string url, string method = "POST",
            CancellationToken cancellationToken = default)
        {
            var opts = Options.FileTransfer;

            var hwr = WebRequest.CreateHttp(url);
            hwr.Timeout = opts.Timeout;
            hwr.UserAgent = opts.UserAgent;
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
