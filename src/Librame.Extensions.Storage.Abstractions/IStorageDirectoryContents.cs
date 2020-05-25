#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 存储目录内容集合接口。
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public interface IStorageDirectoryContents : IEnumerable<IStorageFileInfo>
    {
        /// <summary>
        /// 目录是否存在。
        /// </summary>
        bool Exists { get; }
    }
}
