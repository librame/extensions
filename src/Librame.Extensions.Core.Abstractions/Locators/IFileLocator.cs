#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 文件定位器接口。
    /// </summary>
    public interface IFileLocator : ILocator<string>, IEquatable<IFileLocator>
    {
        /// <summary>
        /// 基础路径。
        /// </summary>
        string BasePath { get; }

        /// <summary>
        /// 文件名。
        /// </summary>
        IFileNameLocator FileName { get; }

        /// <summary>
        /// 文件名字符串。
        /// </summary>
        string FileNameString { get; }


        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回当前 <see cref="IFileLocator"/>。</returns>
        IFileLocator ChangeBasePath(string newBasePath);

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回当前 <see cref="IFileLocator"/>。</returns>
        IFileLocator ChangeFileName(string newFileName);

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="IFileNameLocator"/>。</param>
        /// <returns>返回当前 <see cref="IFileLocator"/>。</returns>
        IFileLocator ChangeFileName(IFileNameLocator newFileName);


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个 <see cref="IFileLocator"/>。
        /// </summary>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        IFileLocator NewBasePath(string newBasePath);

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="IFileLocator"/>。
        /// </summary>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        IFileLocator NewFileName(string newFileName);

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="IFileLocator"/>。
        /// </summary>
        /// <param name="newFileName">给定的新 <see cref="IFileNameLocator"/>。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        IFileLocator NewFileName(IFileNameLocator newFileName);
    }
}
