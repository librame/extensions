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
using System.IO;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Builders;
    using Services;

    /// <summary>
    /// 内部文件写入器。
    /// </summary>
    internal class InternalFileWriter : AbstractService<InternalFileWriter>, IFileWriter
    {
        private readonly StorageBuilderOptions _options;


        /// <summary>
        /// 构造一个 <see cref="InternalFileWriter"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DefaultStorageBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalFileWriter}"/>。</param>
        public InternalFileWriter(IOptions<StorageBuilderOptions> options, ILogger<InternalFileWriter> logger)
            : base(logger)
        {
            _options = options.Value;
        }

        
        /// <summary>
        /// 异步另存为文件。
        /// </summary>
        /// <param name="saveFilePath">给定的保存文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task SaveAsAsync(string saveFilePath, string srcFilePath)
        {
            return Task.Factory.StartNew(() => SaveAs(saveFilePath, srcFilePath));
        }

        /// <summary>
        /// 另存为文件。
        /// </summary>
        /// <param name="saveFilePath">给定的保存文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        public virtual void SaveAs(string saveFilePath, string srcFilePath)
        {
            using (var inputStream = new FileStream(srcFilePath,
                FileMode.Open, FileAccess.Read, FileShare.Read, _options.BufferSize))
            {
                Write(inputStream, saveFilePath);
                Logger.LogDebug($"Save as {srcFilePath} to {saveFilePath}");
            }
        }


        /// <summary>
        /// 异步读取输入流并写入到文件。
        /// </summary>
        /// <param name="inputStream">给定的输入流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task WriteAsync(Stream inputStream, string filePath)
        {
            return Task.Factory.StartNew(() => Write(inputStream, filePath));
        }

        /// <summary>
        /// 读取输入流并写入到文件。
        /// </summary>
        /// <param name="inputStream">给定的输入流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        public virtual void Write(Stream inputStream, string filePath)
        {
            var offset = File.Exists(filePath) ? new FileInfo(filePath).Length : 0;

            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, _options.BufferSize))
            {
                // 输入流偏移定位
                if (inputStream.CanSeek && offset > 0)
                    inputStream.Seek(offset, SeekOrigin.Current);

                var buffer = new byte[_options.BufferSize];
                var readSize = 0;

                // 读取输入流缓冲
                readSize = inputStream.Read(buffer, 0, _options.BufferSize);

                while (readSize > 0)
                {
                    // 写入文件流缓冲
                    fs.Write(buffer, 0, readSize);
                    
                    // 继续读取
                    readSize = inputStream.Read(buffer, 0, readSize);
                }
            }
        }

    }
}
