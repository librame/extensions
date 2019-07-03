#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
        /// 异步下载文件。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string> DownloadFileAsync(string downloadUrl, string filePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步下载文件集合。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string> DownloadFilesAsync(string downloadUrl, string[] filePaths,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUri">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string> UploadFileAsync(string uploadUri, string filePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步上传文件集合。
        /// </summary>
        /// <param name="uploadUri">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string> UploadFilesAsync(string uploadUri, string[] filePaths,
            CancellationToken cancellationToken = default);
    }
}
