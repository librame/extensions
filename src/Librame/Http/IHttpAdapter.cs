#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using JumpKick.HttpLib;
using System;
using System.Net;

namespace Librame.Http
{
    /// <summary>
    /// HTTP 适配器接口。
    /// </summary>
    public interface IHttpAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取网页。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="action">给定的操作方法。</param>
        void GetWeb(string url, Action<string> action);

        /// <summary>
        /// 提交网页。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="body">给定的主体对象（如 JSON 格式对象）。</param>
        void PostWeb(string url, object body);

        /// <summary>
        /// 删除网页。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        void Delete(string url);
        

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="files">给定的文件流集合（可使用 <see cref="HttpHelper.BuildUploadFileStream(System.IO.FileInfo, string)"/> 快速构建）。</param>
        /// <param name="parameters">给定的参数。</param>
        /// <param name="onProgressChanged">给定的进度条事件。</param>
        /// <param name="onUploadComplete">给定的上传完成事件。</param>
        void Upload(string url, NamedFileStream[] files, object parameters, Action<long, long?> onProgressChanged, Action<long> onUploadComplete);


        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="saveFilePath"></param>
        /// <param name="onProgressChanged">给定的进度条事件。</param>
        /// <param name="onSuccess">给定的下载完成事件。</param>
        void Download(string url, string saveFilePath, Action<long, long?> onProgressChanged, Action<WebHeaderCollection> onSuccess);
    }
}
