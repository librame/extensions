#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 文件名定位符。
    /// </summary>
    public class FileNameLocator : AbstractFileNameLocator, IFileNameLocator
    {
        /// <summary>
        /// 构造一个 <see cref="FileNameLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public FileNameLocator(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="FileNameLocator"/> 实例。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        public FileNameLocator(string baseName, string extension)
            :base(baseName, extension)
        {
        }


        /// <summary>
        /// 依据当前文件扩展名与指定的文件基础名，新建一个 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="newBaseName">给定的新文件基础名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public override IFileNameLocator NewBaseName(string newBaseName)
        {
            return new FileNameLocator(newBaseName, Extension);
        }

        /// <summary>
        /// 依据当前文件基础名与指定的文件扩展名，新建一个 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="newExtension">给定的新文件扩展名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public override IFileNameLocator NewExtension(string newExtension)
        {
            return new FileNameLocator(BaseName, newExtension);
        }


        /// <summary>
        /// 比较指定的对象是否与当前实例相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is IFileNameLocator locator)
                return Equals(locator);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        /// <summary>
        /// 比较两个 <see cref="FileNameLocator"/> 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="b">给定的 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public static bool operator ==(FileNameLocator a, FileNameLocator b)
        {
            return a.ToString() == b.ToString();
        }

        /// <summary>
        /// 比较两个 <see cref="FileNameLocator"/> 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="b">给定的 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回是否不等的布尔值。</returns>
        public static bool operator !=(FileNameLocator a, FileNameLocator b)
        {
            return !(a == b);
        }


        /// <summary>
        /// 隐式转换。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        public static implicit operator string(FileNameLocator locator)
        {
            return locator.ToString();
        }

        /// <summary>
        /// 显式转换。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public static explicit operator FileNameLocator(string fileName)
        {
            return new FileNameLocator(fileName);
        }

    }
}
