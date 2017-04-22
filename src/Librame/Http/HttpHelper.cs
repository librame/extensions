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
using System.IO;

namespace Librame.Http
{
    /// <summary>
    /// HTTP 助手。
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 建立上传文件流。
        /// </summary>
        /// <param name="filename">给定的文件名。</param>
        /// <param name="title">给定的标题（可选；默认为不包含扩展名的文件名）。</param>
        /// <returns>返回 <see cref="NamedFileStream"/>。</returns>
        public static NamedFileStream BuildUploadFileStream(string filename, string title = null)
        {
            return BuildUploadFileStream(new FileInfo(filename), title);
        }

        /// <summary>
        /// 建立上传文件流。
        /// </summary>
        /// <param name="file">给定的文件信息。</param>
        /// <param name="title">给定的标题（可选；默认为不包含扩展名的文件名）。</param>
        /// <returns>返回 <see cref="NamedFileStream"/>。</returns>
        public static NamedFileStream BuildUploadFileStream(FileInfo file, string title = null)
        {
            if (string.IsNullOrEmpty(title))
                title = file.Name.Replace(file.Extension, string.Empty);

            return new NamedFileStream(title, file.Name, "application/octet-stream", file.OpenRead());
        }

    }
}
