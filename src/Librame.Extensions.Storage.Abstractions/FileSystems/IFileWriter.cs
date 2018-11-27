#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 文件写入器接口。
    /// </summary>
    public interface IFileWriter : IStorageService
    {
        /// <summary>
        /// 异步另存为文件。
        /// </summary>
        /// <param name="saveFilePath">给定的保存文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        /// <returns>返回一个异步操作。</returns>
        Task SaveAsAsync(string saveFilePath, string srcFilePath);

        /// <summary>
        /// 另存为文件。
        /// </summary>
        /// <param name="saveFilePath">给定的保存文件路径。</param>
        /// <param name="srcFilePath">给定的源文件路径。</param>
        void SaveAs(string saveFilePath, string srcFilePath);


        /// <summary>
        /// 异步读取输入流并写入到文件。
        /// </summary>
        /// <param name="inputStream">给定的输入流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <returns>返回一个异步操作。</returns>
        Task WriteAsync(Stream inputStream, string filePath);

        /// <summary>
        /// 读取输入流并写入到文件。
        /// </summary>
        /// <param name="inputStream">给定的输入流。</param>
        /// <param name="filePath">给定的文件路径。</param>
        void Write(Stream inputStream, string filePath);
    }
}
