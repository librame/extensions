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
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="File"/> 实用工具。
    /// </summary>
    public class FileUtility
    {
        /// <summary>
        /// 读取内容。
        /// </summary>
        /// <param name="path">给定的文件路径。</param>
        /// <param name="encoding">给定读取的字符编码。</param>
        /// <returns>返回字符串。</returns>
        public static string ReadContent(string path, Encoding encoding)
        {
            // 仅用打开模式，当文件不存在时以便引发异常
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (var sr = new StreamReader(fs, encoding))
                {
                    return sr.ReadToEnd();
                }
            }

            //using (var sr = new StreamReader(Info.Name, Encoding))
            //{
            //    return sr.ReadToEnd();
            //}
        }

        /// <summary>
        /// 写入内容。
        /// </summary>
        /// <param name="content">给定的文件设置内容。</param>
        /// <param name="path">给定的文件路径。</param>
        /// <param name="encoding">给定读取的字符编码。</param>
        /// <returns>返回字符串。</returns>
        public static void WriteContent(string content, string path, Encoding encoding)
        {
            // 如果使用 FileMode.OpenOrCreate 模式，则在 JSON 序列化保存时会莫名多出部分 JSON 字符串而导致加载发生异常
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, encoding))
                {
                    sw.Write(content);
                }
            }

            //using (var sw = new StreamWriter(Info.Name))
            //{
            //    sw.Write(content);
            //}
        }


        /// <summary>
        /// 格式化文件大小为自适应友好的字符串形式。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatAdaptiveFileSize(long fileSize)
        {
            if (fileSize < 1)
                return IntUtility.FormatFileSizeUnit(fileSize);
            else if (fileSize >= (long)FileSizeUnit.Max)
                return IntUtility.FormatFileSizeUnit(fileSize, FileSizeUnit.Max);
            else if (fileSize >= (long)FileSizeUnit.TiB)
                return IntUtility.FormatFileSizeUnit(fileSize, FileSizeUnit.TiB);
            else if (fileSize >= (long)FileSizeUnit.GiB)
                return IntUtility.FormatFileSizeUnit(fileSize, FileSizeUnit.GiB);
            else if (fileSize >= (long)FileSizeUnit.MiB)
                return IntUtility.FormatFileSizeUnit(fileSize, FileSizeUnit.MiB);
            else if (fileSize >= (long)FileSizeUnit.KiB)
                return IntUtility.FormatFileSizeUnit(fileSize, FileSizeUnit.KiB);
            else
                return string.Format("{0} bytes", fileSize);
        }

    }


    /// <summary>
    /// <see cref="FileUtility"/> 静态扩展。
    /// </summary>
    public static class FileUtilityExtensions
    {
        /// <summary>
        /// 格式化文件大小为自适应友好的字符串形式。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatAdaptiveFileSize(this long fileSize)
        {
            return FileUtility.FormatAdaptiveFileSize(fileSize);
        }

    }

}
