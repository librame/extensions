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
    /// 默认 HTTP 适配器。
    /// </summary>
    public class DefaultHttpAdapter : AbstractHttpAdapter, IHttpAdapter
    {
        /// <summary>
        /// 获取网页。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="action">给定的操作方法。</param>
        public virtual void GetWeb(string url, Action<string> action)
        {
            JumpKick.HttpLib.Http.Get(url).OnSuccess(action).Go();
        }

        /// <summary>
        /// 提交网页。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="body">给定的主体对象（如 JSON 格式对象）。</param>
        public virtual void PostWeb(string url, object body)
        {
            JumpKick.HttpLib.Http.Post(url).Form(body).Go();
        }

        /// <summary>
        /// 删除网页。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        public virtual void Delete(string url)
        {
            JumpKick.HttpLib.Http.Delete(url).Go();
        }


        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="files">给定的文件流集合（可使用 <see cref="HttpHelper.BuildUploadFileStream(System.IO.FileInfo, string)"/> 快速构建）。</param>
        /// <param name="parameters">给定的参数。</param>
        /// <param name="onProgressChanged">给定的进度条事件。</param>
        /// <param name="onUploadComplete">给定的上传完成事件。</param>
        public virtual void Upload(string url, NamedFileStream[] files, object parameters, Action<long, long?> onProgressChanged, Action<long> onUploadComplete)
        {
            JumpKick.HttpLib.Http.Post(url)
                .Upload(files, parameters, onProgressChanged, onUploadComplete).Go();

            //onProgressChanged: (bytesSent, totalBytes) =>
            //{
            //    Console.WriteLine("Uploading: " + (bytesSent / totalBytes) * 100 + "% completed");
            //})

            //onUploadComplete: result =>
            //{
            //    Console.WriteLine(result);
            //}
        }


        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="saveFilePath"></param>
        /// <param name="onProgressChanged">给定的进度条事件。</param>
        /// <param name="onSuccess">给定的下载完成事件。</param>
        public virtual void Download(string url, string saveFilePath, Action<long, long?> onProgressChanged, Action<WebHeaderCollection> onSuccess)
        {
            JumpKick.HttpLib.Http.Get(url)
                .DownloadTo(saveFilePath, onProgressChanged, onSuccess).Go();
            
            //onProgressChanged: (bytesCopied, totalBytes) =>
            //{
            //    if (totalBytes.HasValue)
            //    {
            //        ("Downloaded: " + (bytesCopied / totalBytes) * 100 + "%");
            //    }
            //    Console.Write("Downloaded: " + bytesCopied.ToString() + " bytes");
            //}

            //onSuccess: (headers) =>
            //{
            //    UpdateText("Download Complete");
            //}
        }

    }
}
