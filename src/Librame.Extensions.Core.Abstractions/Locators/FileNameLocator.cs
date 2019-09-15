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
    /// 文件名定位器。
    /// </summary>
    public class FileNameLocator : AbstractLocator<string>
    {
        /// <summary>
        /// 构造一个 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public FileNameLocator(string fileName)
            : base(Path.GetFileName(fileName))
        {
            Extension = Path.GetExtension(fileName);
            BaseName = RawSource.TrimEnd(Extension);
        }

        /// <summary>
        /// 构造一个 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        protected internal FileNameLocator(string baseName, string extension)
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
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public FileNameLocator ChangeBaseName(string newBaseName)
        {
            BaseName = newBaseName.NotNullOrEmpty(nameof(newBaseName));
            return this;
        }

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public FileNameLocator ChangeExtension(string newExtension)
        {
            Extension = newExtension.NotNullOrEmpty(nameof(newExtension));
            return this;
        }


        /// <summary>
        /// 依据当前文件定位器的扩展名与指定的基础名，新建一个 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public FileNameLocator NewBaseName(string newBaseName)
            => new FileNameLocator(newBaseName, Extension);

        /// <summary>
        /// 依据当前文件定位器的基础名与指定的扩展名，新建一个 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public FileNameLocator NewExtension(string newExtension)
            => new FileNameLocator(BaseName, newExtension);


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(FileNameLocator other)
            => Source == other?.Source;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is FileNameLocator other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="b">给定的 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(FileNameLocator a, FileNameLocator b)
            => a.Equals(b);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="b">给定的 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(FileNameLocator a, FileNameLocator b)
            => !a.Equals(b);


        /// <summary>
        /// 隐式转换。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        public static implicit operator string(FileNameLocator locator)
            => locator?.ToString();

        /// <summary>
        /// 显式转换。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public static explicit operator FileNameLocator(string fileName)
            => new FileNameLocator(fileName);
    }
}
