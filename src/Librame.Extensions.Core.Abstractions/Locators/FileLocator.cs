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
    /// 文件定位器。
    /// </summary>
    public class FileLocator : AbstractLocator<string>
    {
        /// <summary>
        /// 构造一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public FileLocator(string fileName)
            : base(fileName)
        {
            FileName = CreateFileNameLocator(fileName);
            BasePath = Path.GetDirectoryName(fileName);
        }

        /// <summary>
        /// 构造一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        protected FileLocator(string fileName, string basePath)
            : base(basePath.CombinePath(fileName))
        {
            FileName = CreateFileNameLocator(fileName);
            BasePath = basePath;
        }

        /// <summary>
        /// 构造一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        protected FileLocator(FileNameLocator fileName, string basePath)
            : base(basePath.CombinePath(fileName.ToString()))
        {
            FileName = fileName;
            BasePath = basePath;
        }


        /// <summary>
        /// 基础路径。
        /// </summary>
        public string BasePath { get; private set; }

        /// <summary>
        /// 文件名。
        /// </summary>
        public FileNameLocator FileName { get; private set; }

        /// <summary>
        /// 文件名字符串。
        /// </summary>
        public string FileNameString
            => FileName.ToString();


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => BasePath.CombinePath(FileName.ToString());


        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回当前 <see cref="FileLocator"/>。</returns>
        public FileLocator ChangeBasePath(string newBasePath)
        {
            BasePath = newBasePath.NotNullOrEmpty(nameof(newBasePath));
            return this;
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回当前 <see cref="FileLocator"/>。</returns>
        public FileLocator ChangeFileName(string newFileName)
        {
            var newFileNameLocator = CreateFileNameLocator(newFileName);
            return ChangeFileName(newFileNameLocator);
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回当前 <see cref="FileLocator"/>。</returns>
        public FileLocator ChangeFileName(FileNameLocator newFileName)
        {
            FileName = newFileName.NotNull(nameof(newFileName));
            return this;
        }


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public FileLocator NewBasePath(string newBasePath)
            => new FileLocator(FileName, newBasePath);

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public FileLocator NewFileName(string newFileName)
            => new FileLocator(newFileName, BasePath);

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public FileLocator NewFileName(FileNameLocator newFileName)
            => new FileLocator(newFileName, BasePath);


        /// <summary>
        /// 创建 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        private FileNameLocator CreateFileNameLocator(string fileName)
            => new FileNameLocator(fileName);


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(FileLocator other)
            => Source == other?.Source;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is FileLocator other) ? Equals(other) : false;


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
        /// <param name="a">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="b">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(FileLocator a, FileLocator b)
            => a.Equals(b);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="b">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(FileLocator a, FileLocator b)
            => !a.Equals(b);


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        public static implicit operator string(FileLocator locator)
            => locator.ToString();

        /// <summary>
        /// 显式转换为文件定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public static explicit operator FileLocator(string fileName)
            => new FileLocator(fileName);
    }
}
