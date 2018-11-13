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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 文件系统服务接口。
    /// </summary>
    public interface IFileSystemService : IStorageService, IDisposable
    {
        /// <summary>
        /// 文件读取器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IFileReader"/>。
        /// </value>
        IFileReader FileReader { get; }

        /// <summary>
        /// 文件写入器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IFileWriter"/>。
        /// </value>
        IFileWriter FileWriter { get; }

        /// <summary>
        /// 平台描述符。
        /// </summary>
        /// <value>
        /// 返回 <see cref="PlatformDescriptor"/>。
        /// </value>
        PlatformDescriptor Platform { get; }


        /// <summary>
        /// 异步加载存储磁盘信息。
        /// </summary>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>（可选；默认使用吉字节单位）。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回一个包含 <see cref="IReadOnlyList{DriverDescriptor}"/> 的异步操作。</returns>
        Task<IReadOnlyList<DriverDescriptor>> LoadDriversAsync(CapacityUnitFormat format = CapacityUnitFormat.GByte,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary);

        /// <summary>
        /// 加载存储磁盘信息。
        /// </summary>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>（可选；默认使用吉字节单位）。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{DriverDescriptor}"/>。</returns>
        IReadOnlyList<DriverDescriptor> LoadDrivers(CapacityUnitFormat format = CapacityUnitFormat.GByte,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary);


        /// <summary>
        /// 异步加载指定目录集合信息。
        /// </summary>
        /// <param name="filter">给定的文件过滤规则。</param>
        /// <param name="directories">给定的目录集合。</param>
        /// <returns>返回一个包含 <see cref="IReadOnlyList{DirectoryDescriptor}"/> 的异步操作。</returns>
        Task<IReadOnlyList<DirectoryDescriptor>> LoadDirectoriesAsync(string filter, params string[] directories);

        /// <summary>
        /// 加载指定目录集合信息。
        /// </summary>
        /// <param name="filter">给定的文件过滤规则。</param>
        /// <param name="directories">给定的目录集合。</param>
        /// <returns>返回 <see cref="IReadOnlyList{DirectoryDescriptor}"/>。</returns>
        IReadOnlyList<DirectoryDescriptor> LoadDirectories(string filter, params string[] directories);
    }
}
