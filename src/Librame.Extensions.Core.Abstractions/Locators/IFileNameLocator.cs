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
    /// 文件名定位器接口。
    /// </summary>
    public interface IFileNameLocator : ILocator<string>, IEquatable<IFileNameLocator>
    {
        /// <summary>
        /// 基础名。
        /// </summary>
        string BaseName { get; }

        /// <summary>
        /// 扩展名。
        /// </summary>
        string Extension { get; }


        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回当前 <see cref="IFileNameLocator"/>。</returns>
        IFileNameLocator ChangeBaseName(string newBaseName);

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回当前 <see cref="IFileNameLocator"/>。</returns>
        IFileNameLocator ChangeExtension(string newExtension);


        /// <summary>
        /// 依据当前文件定位器的扩展名与指定的基础名，新建一个 <see cref="IFileNameLocator"/>。
        /// </summary>
        /// <param name="newBaseName">给定的新基础名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        IFileNameLocator NewBaseName(string newBaseName);

        /// <summary>
        /// 依据当前文件定位器的基础名与指定的扩展名，新建一个 <see cref="IFileNameLocator"/>。
        /// </summary>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        IFileNameLocator NewExtension(string newExtension);
    }
}
