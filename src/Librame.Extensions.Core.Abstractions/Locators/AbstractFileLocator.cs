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
        public string FileName { get; private set; }
        
        /// <summary>
        /// 基础路径。
        /// </summary>
        public string BasePath { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source => BasePath.CombinePath(FileName);


        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回当前 <see cref="IFileLocator"/>。</returns>
        public virtual IFileLocator ChangeBasePath(string newBasePath)
        {
            BasePath = newBasePath.NotNullOrEmpty(nameof(newBasePath));
            return this;
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回当前 <see cref="IFileLocator"/>。</returns>
        public virtual IFileLocator ChangeFileName(string newFileName)
        {
            FileName = newFileName.NotNullOrEmpty(nameof(newFileName));
            return this;
        }


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个文件定位器。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public abstract IFileLocator NewBasePath(string newBasePath);

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个文件定位器。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public abstract IFileLocator NewFileName(string newFileName);


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IFileLocator other)
        {
            return Source == other.Source;
        }


        /// <summary>
        /// 转换为文件源路径。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Source;
        }

    }
}
