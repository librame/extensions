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

namespace Librame.Locators
{
    /// <summary>
    /// 文件定位器接口。
    /// </summary>
    public interface IFileLocator : ILocator<string>, IEquatable<IFileLocator>
    {
        /// <summary>
        /// 文件名。
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// 基础路径。
        /// </summary>
        string BasePath { get; }


        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="changeFactory">给定的改变工厂方法。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        IFileLocator ChangeBasePath(Func<string, string> changeFactory);
        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        IFileLocator ChangeBasePath(string basePath);
    }
}
