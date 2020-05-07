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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage.Services
{
    using Core.Combiners;
    using Core.Services;

    /// <summary>
    /// 文件传输服务接口。
    /// </summary>
    public interface IFileTransferService : IService
    {
        /// <summary>
        /// 字符编码。
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// 使用访问令牌。
        /// </summary>
        bool UseAccessToken { get; set; }

        /// <summary>
        /// 使用授权码。
        /// </summary>
        bool UseAuthorizationCode { get; set; }

        /// <summary>
        /// 使用 Cookie 值。
        /// </summary>
        bool UseCookieValue { get; set; }

        /// <summary>
        /// 使用断点续传（默认使用）。
        /// </summary>
        bool UseBreakpointResume { get; set; }

        /// <summary>
        /// 进度动作（传入参数依次为总长度、当前位置）。
        /// </summary>
        Action<long, long> ProgressAction { get; set; }


        /// <summary>
        /// 异步下载文件。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含 <see cref="FilePathCombiner"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        Task<FilePathCombiner> DownloadFileAsync(string downloadUrl, string savePath,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUrl">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        Task<string> UploadFileAsync(string uploadUrl, string filePath,
            CancellationToken cancellationToken = default);
    }
}
