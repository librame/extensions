#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 物理存储文件提供程序。
    /// </summary>
    public class PhysicalStorageFileProvider : PhysicalFileProvider, IStorageFileProvider
    {
        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageFileProvider"/>。
        /// </summary>
        /// <param name="root">给定的根路径。</param>
        public PhysicalStorageFileProvider(string root)
            : this(root, ExclusionFilters.Sensitive)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageFileProvider"/>。
        /// </summary>
        /// <param name="root">给定的根路径。</param>
        /// <param name="filters">指定排除哪些文件或目录。</param>
        public PhysicalStorageFileProvider(string root, ExclusionFilters filters)
            : base(root, filters)
        {
        }


        ///// <summary>
        ///// 析构 <see cref="PhysicalStorageFileProvider"/>。
        ///// </summary>
        //~PhysicalStorageFileProvider()
        //{
        //    Dispose(false);
        //}


        /// <summary>
        /// 获取文件信息。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <returns>返回 <see cref="IStorageFileInfo"/>。</returns>
        public new IStorageFileInfo GetFileInfo(string subpath)
        {
            var fileInfo = base.GetFileInfo(subpath);
            return new PhysicalStorageFileInfo(fileInfo);
        }

        /// <summary>
        /// 获取目录内容集合。
        /// </summary>
        /// <param name="subpath">给定的子路径。</param>
        /// <returns>返回 <see cref="IStorageDirectoryContents"/>。</returns>
        public new IStorageDirectoryContents GetDirectoryContents(string subpath)
        {
            var contents = base.GetDirectoryContents(subpath);
            return new PhysicalStorageDirectoryContents(contents);
        }
    }
}
