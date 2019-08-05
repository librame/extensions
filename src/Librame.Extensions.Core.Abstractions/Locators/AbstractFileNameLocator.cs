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
    /// 抽象文件名定位器。
    /// </summary>
    public abstract class AbstractFileNameLocator : AbstractLocator<string>, IFileNameLocator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractFileNameLocator"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public AbstractFileNameLocator(string fileName)
            : base(Path.GetFileName(fileName))
        {
            Extension = Path.GetExtension(fileName);
            BaseName = RawSource.TrimEnd(Extension);
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractFileNameLocator"/>。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        protected AbstractFileNameLocator(string baseName, string extension)
            : base(baseName + extension)
        {
            Extension = extension; // 存在有些没扩展名的文件名
            BaseName = baseName.NotNullOrEmpty(nameof(baseName));
        }


        /// <summary>
        /// 基础名。
        /// </summary>
        public string BaseName { get; private set; }

        /// <summary>
        /// 扩展名。
        /// </summary>
        public string Extension { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => BaseName + Extension;


        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public IFileNameLocator ChangeBaseName(string newBaseName)
        {
            BaseName = newBaseName.NotNullOrEmpty(nameof(newBaseName));
            return this;
        }

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public IFileNameLocator ChangeExtension(string newExtension)
        {
            Extension = newExtension.NotNullOrEmpty(nameof(newExtension));
            return this;
        }


        /// <summary>
        /// 依据当前文件定位器的扩展名与指定的基础名，新建一个 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public abstract IFileNameLocator NewBaseName(string newBaseName);

        /// <summary>
        /// 依据当前文件定位器的基础名与指定的扩展名，新建一个 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public abstract IFileNameLocator NewExtension(string newExtension);


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IFileNameLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IFileNameLocator other)
        {
            return Source == other.Source;
        }


        /// <summary>
        /// 转换为文件名。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Source;
        }

    }
}
