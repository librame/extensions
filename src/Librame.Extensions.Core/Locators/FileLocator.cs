﻿#region License

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
    /// 文件定位器。
    /// </summary>
    public class FileLocator : AbstractFileLocator, IFileLocator
    {
        /// <summary>
        /// 构造一个 <see cref="FileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public FileLocator(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="FileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        protected FileLocator(string fileName, string basePath)
            : base(fileName, basePath)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="FileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        protected FileLocator(IFileNameLocator fileName, string basePath)
            : base(fileName, basePath)
        {
        }


        /// <summary>
        /// 创建 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        protected override IFileNameLocator CreateFileNameLocator(string fileName)
        {
            return new FileNameLocator(fileName);
        }


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个文件定位器。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public override IFileLocator NewBasePath(string newBasePath)
        {
            return new FileLocator(FileName, newBasePath);
        }

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个文件定位器。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public override IFileLocator NewFileName(string newFileName)
        {
            return new FileLocator(newFileName, BasePath);
        }

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="IFileLocator"/> 实例。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="IFileNameLocator"/>。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public override IFileLocator NewFileName(IFileNameLocator newFileName)
        {
            return new FileLocator(newFileName, BasePath);
        }


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        public static implicit operator string(FileLocator locator)
        {
            return locator.ToString();
        }

        /// <summary>
        /// 显式转换为文件定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public static explicit operator FileLocator(string fileName)
        {
            return new FileLocator(fileName);
        }

    }
}
