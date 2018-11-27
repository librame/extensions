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
using System.IO;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 文件读取器接口。
    /// </summary>
    public interface IFileReader : IStorageService
    {
        /// <summary>
        /// 异步复制文件。
        /// </summary>
        /// <param name="copyFilePath">给定的副本文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task CopyAsync(string copyFilePath, string srcFilePath, Action<long> fileLengthAction = null);

        /// <summary>
        /// 复制文件。
        /// </summary>
        /// <param name="copyFilePath">给定的副本文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        void Copy(string copyFilePath, string srcFilePath, Action<long> fileLengthAction = null);


        /// <summary>
        /// 异步读取文件到输出流。
        /// </summary>
        /// <param name="outputStream">给定的输出流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        /// <param name="offset">给定的续传偏移量（可选；默认从开头读取）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task ReadAsync(Stream outputStream, string filePath, Action<long> fileLengthAction = null, long offset = 0);

        /// <summary>
        /// 读取文件到输出流。
        /// </summary>
        /// <param name="outputStream">给定的输出流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        /// <param name="offset">给定的续传偏移量（可选；默认从开头读取）。</param>
        void Read(Stream outputStream, string filePath, Action<long> fileLengthAction = null, long offset = 0);
    }
}
