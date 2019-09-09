#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 存储文件信息。
    /// </summary>
    public interface IStorageFileInfo : IFileInfo
    {
        /// <summary>
        /// 创建写入流。
        /// </summary>
        /// <returns>返回 <see cref="Stream"/>。</returns>
        Stream CreateWriteStream();
    }
}
