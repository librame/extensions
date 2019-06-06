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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象文件定位器。
    /// </summary>
    public abstract class AbstractFileLocator : AbstractLocator<string>, IFileLocator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractFileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public AbstractFileLocator(string fileName)
            : base(fileName)
        {
            FileName = Path.GetFileName(fileName);
            BasePath = Path.GetDirectoryName(fileName);
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractFileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        protected AbstractFileLocator(string fileName, string basePath)
            : base(basePath.CombinePath(fileName))
        {
            FileName = fileName;
            BasePath = basePath;
        }


        /// <summary>
        /// 文件名。
        /// </summary>
        public string FileName { get; }
        
        /// <summary>
        /// 基础路径。
        /// </summary>
        public string BasePath { get; }


        /// <summary>
        /// 获取源。
        /// </summary>
        /// <returns>返回源实例。</returns>
        public override string GetSource()
        {
            return BasePath.CombinePath(FileName);
        }


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IFileLocator other)
        {
            return GetSource() == other.GetSource();
        }


        /// <summary>
        /// 转换为文件源路径。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return GetSource();
        }

    }
}
