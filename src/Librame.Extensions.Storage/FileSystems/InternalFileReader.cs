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
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Builders;
    using Services;

    /// <summary>
    /// 内部文件读取器。
    /// </summary>
    internal class InternalFileReader : AbstractService<InternalFileReader>, IFileReader
    {
        private readonly StorageBuilderOptions _options;


        /// <summary>
        /// 构造一个 <see cref="InternalFileReader"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DefaultStorageBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalFileReader}"/>。</param>
        public InternalFileReader(IOptions<StorageBuilderOptions> options, ILogger<InternalFileReader> logger)
            : base(logger)
        {
            _options = options.Value;
        }


        /// <summary>
        /// 异步复制文件。
        /// </summary>
        /// <param name="copyFilePath">给定的副本文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task CopyAsync(string copyFilePath, string srcFilePath, Action<long> fileLengthAction = null)
        {
            return Task.Factory.StartNew(() => Copy(copyFilePath, srcFilePath, fileLengthAction));
        }

        /// <summary>
        /// 复制文件。
        /// </summary>
        /// <param name="copyFilePath">给定的副本文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        public virtual void Copy(string copyFilePath, string srcFilePath, Action<long> fileLengthAction = null)
        {
            using (var outputStream = new FileStream(copyFilePath,
                FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, _options.BufferSize))
            {
                Read(outputStream, srcFilePath, fileLengthAction);
                Logger.LogDebug($"Copy {srcFilePath} to {copyFilePath}");
            }
        }


        /// <summary>
        /// 异步读取文件到输出流。
        /// </summary>
        /// <param name="outputStream">给定的输出流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        /// <param name="offset">给定的续传偏移量（可选；默认从开头读取）。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task ReadAsync(Stream outputStream, string filePath, Action<long> fileLengthAction = null, long offset = 0)
        {
            return Task.Factory.StartNew(() => Read(outputStream, filePath, fileLengthAction, offset));
        }

        /// <summary>
        /// 读取文件到输出流。
        /// </summary>
        /// <param name="outputStream">给定的输出流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="fileLengthAction">给定的文件长度动作（可选）。</param>
        /// <param name="offset">给定的续传偏移量（可选；默认从开头读取）。</param>
        public virtual void Read(Stream outputStream, string filePath, Action<long> fileLengthAction = null, long offset = 0)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, _options.BufferSize))
            {
                fileLengthAction?.Invoke(fs.Length);

                // 文件偏移定位
                if (fs.CanSeek && offset > 0)
                    fs.Seek(offset, SeekOrigin.Current);

                var buffer = new byte[_options.BufferSize];
                var readSize = 0;

                // 读取文件流缓冲
                readSize = fs.Read(buffer, 0, _options.BufferSize);

                while (readSize > 0)
                {
                    // 写入输出流缓冲
                    outputStream.Write(buffer, 0, readSize);

                    // 继续读取
                    readSize = fs.Read(buffer, 0, readSize);
                }
            }
        }

    }
}
