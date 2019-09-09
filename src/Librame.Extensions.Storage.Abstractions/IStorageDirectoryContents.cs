#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 存储目录内容集合接口。
    /// </summary>
    public interface IStorageDirectoryContents : IEnumerable<IStorageFileInfo>
    {
        /// <summary>
        /// 目录是否存在。
        /// </summary>
        bool Exists { get; }
    }
}
