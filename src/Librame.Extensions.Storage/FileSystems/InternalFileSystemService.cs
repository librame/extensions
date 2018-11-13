#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Services;

    /// <summary>
    /// 内部文件系统服务。
    /// </summary>
    internal class InternalFileSystemService : AbstractService<InternalFileSystemService>, IFileSystemService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalFileSystemService"/> 实例。
        /// </summary>
        /// <param name="fileReader">给定的 <see cref="IFileReader"/>。</param>
        /// <param name="fileWriter">给定的 <see cref="IFileWriter"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalFileSystemService}"/>。</param>
        public InternalFileSystemService(IFileReader fileReader, IFileWriter fileWriter,
            ILogger<InternalFileSystemService> logger)
            : base(logger)
        {
            FileReader = fileReader.NotDefault(nameof(fileReader));
            FileWriter = fileWriter.NotDefault(nameof(fileWriter));
        }

        
        /// <summary>
        /// 文件读取器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IFileReader"/>。
        /// </value>
        public IFileReader FileReader { get; }

        /// <summary>
        /// 文件写入器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IFileWriter"/>。
        /// </value>
        public IFileWriter FileWriter { get; }


        /// <summary>
        /// 环境信息。
        /// </summary>
        /// <value>
        /// 返回 <see cref="PlatformDescriptor"/>。
        /// </value>
        public virtual PlatformDescriptor Platform => new PlatformDescriptor();


        /// <summary>
        /// 异步加载存储磁盘信息。
        /// </summary>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>（可选；默认使用吉字节单位）。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回一个包含 <see cref="IReadOnlyList{DriverDescriptor}"/> 的异步操作。</returns>
        public Task<IReadOnlyList<DriverDescriptor>> LoadDriversAsync(CapacityUnitFormat format = CapacityUnitFormat.GByte,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            return Task.Factory.StartNew(() => LoadDrivers(format, notation));
        }

        private static ConcurrentDictionary<string, DriverDescriptor> _drivers
            = new ConcurrentDictionary<string, DriverDescriptor>();
        /// <summary>
        /// 加载存储磁盘信息。
        /// </summary>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>（可选；默认使用吉字节单位）。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{DriverDescriptor}"/>。</returns>
        public IReadOnlyList<DriverDescriptor> LoadDrivers(CapacityUnitFormat format = CapacityUnitFormat.GByte,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            if (_drivers.IsEmpty())
            {
                var drives = DriveInfo.GetDrives();
                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        if (!_drivers.ContainsKey(drive.Name))
                        {
                            var descriptor = new DriverDescriptor(drive, format, notation);
                            _drivers.AddOrUpdate(drive.Name, descriptor, (n, d) => descriptor);
                            Logger.LogDebug($"Name={drive.Name},TotalSize={descriptor.FormatTotalSize},TotalUsageSpace={descriptor.FormatTotalUsageSpace},TotalFreeSpace={descriptor.FormatTotalFreeSpace}");
                        }
                    }
                }
            }

            return _drivers.Values.AsReadOnlyList();
        }


        /// <summary>
        /// 异步加载指定目录集合信息。
        /// </summary>
        /// <param name="filter">给定的文件过滤规则。</param>
        /// <param name="directories">给定的目录集合。</param>
        /// <returns>返回一个包含 <see cref="IReadOnlyList{DirectoryDescriptor}"/> 的异步操作。</returns>
        public Task<IReadOnlyList<DirectoryDescriptor>> LoadDirectoriesAsync(string filter, params string[] directories)
        {
            return Task.Factory.StartNew(() => LoadDirectories(filter, directories));
        }
        
        private static ConcurrentDictionary<string, KeyValuePair<DirectoryDescriptor, FileSystemWatcher>> _directories
            = new ConcurrentDictionary<string, KeyValuePair<DirectoryDescriptor, FileSystemWatcher>>();
        /// <summary>
        /// 加载指定目录集合信息。
        /// </summary>
        /// <param name="filter">给定的文件过滤规则。</param>
        /// <param name="directories">给定的目录集合。</param>
        /// <returns>返回 <see cref="IReadOnlyList{DirectoryDescriptor}"/>。</returns>
        public IReadOnlyList<DirectoryDescriptor> LoadDirectories(string filter, params string[] directories)
        {
            if (directories.IsEmpty())
                directories = new string[] { AppDomain.CurrentDomain.BaseDirectory };

            foreach (var dir in directories.Select(str => new DirectoryInfo(str)))
            {
                if (!_directories.ContainsKey(dir.FullName))
                {
                    var descriptor = new DirectoryDescriptor(dir, filter);
                    var pair = new KeyValuePair<DirectoryDescriptor, FileSystemWatcher>(descriptor, CreateWatcher(descriptor, filter));
                    
                    _directories.AddOrUpdate(dir.FullName, pair, (n, p) => pair);
                    Logger.LogDebug($"FullName={dir.FullName},Size={descriptor.FormatSize}");
                }
            }

            return _directories.Values.Select(pair => pair.Key).AsReadOnlyList();
        }


        private bool _isCreated = false;
        private bool _isChanged = false;
        /// <summary>
        /// 创建监视器。
        /// </summary>
        /// <param name="directory">给定的目录。</param>
        /// <param name="filter">给定的文件过滤规则。</param>
        /// <returns>返回 <see cref="FileSystemWatcher"/>。</returns>
        private FileSystemWatcher CreateWatcher(DirectoryDescriptor directory, string filter)
        {
            var watcher = new FileSystemWatcher(directory.Path, filter);

            watcher.NotifyFilter = NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.LastWrite
                | NotifyFilters.Size;

            watcher.Created += (sender, e) =>
            {
                _isCreated = true;

                try
                {
                    // 新增
                    if (Directory.Exists(e.FullPath))
                    {
                        var newInfo = new DirectoryInfo(e.FullPath);
                        var newDir = new DirectoryDescriptor(newInfo, filter);

                        var parentPair = _directories.Values.FirstOrDefault(val => val.Key.LookupDirectory(newInfo.Parent.FullName).IsNotDefault());
                        newDir.Parent = parentPair.Key;

                        if (newDir.Parent.IsNotDefault())
                        {
                            newDir.Parent.Subdirs.Add(newDir);
                            newDir.Parent.Size += newDir.Size;
                            Logger.LogDebug($"Add new directory: FullName={newDir.Path},Size={newDir.FormatSize}");
                        }
                    }
                    else if (File.Exists(e.FullPath))
                    {
                        var newInfo = new FileInfo(e.FullPath);
                        var newFile = new FileDescriptor(newInfo);

                        var newPair = _directories.Values.FirstOrDefault(val => val.Key.LookupDirectory(newInfo.DirectoryName).IsNotDefault());
                        newFile.Directory = newPair.Key;

                        if (newFile.Directory.IsNotDefault())
                        {
                            newFile.Directory.Subfiles.Add(newFile);
                            newFile.Directory.Size += newInfo.Length;
                            Logger.LogDebug($"Add new file: FullName={newFile.Path},Size={newFile.FormatSize}");
                        }
                    }
                    else
                    {
                        //
                    }
                }
                catch (Exception)
                {
                    _isCreated = false;

                    (sender as FileSystemWatcher).EnableRaisingEvents = false;
                }
            };

            watcher.Deleted += (sender, e) =>
            {
                try
                {
                    // 删除
                    DirectoryDescriptor existDir = null;
                    FileDescriptor existFile = null;

                    foreach (var val in _directories.Values)
                    {
                        existDir = val.Key.LookupDirectory(e.FullPath);
                        if (existDir.IsNotDefault())
                            break;

                        existFile = val.Key.LookupFile(e.FullPath);
                        if (existFile.IsNotDefault())
                            break;
                    }

                    // 目录
                    if (existDir.IsNotDefault())
                    {
                        var parentDir = existDir.Parent;
                        if (parentDir.IsNotDefault())
                        {
                            parentDir.Subdirs.Remove(existDir);
                            parentDir.Size -= existDir.Size;
                            Logger.LogDebug($"Remove directory: FullName={existDir.Path},Size={existDir.FormatSize}");
                        }
                    }
                    // 文件
                    else if (existFile.IsNotDefault())
                    {
                        existDir = existFile?.Directory;
                        if (existDir.IsNotDefault())
                        {
                            existDir.Subfiles.Remove(existFile);
                            existDir.Size -= existFile.Size;
                            Logger.LogDebug($"Remove file: FullName={existDir.Path},Size={existDir.FormatSize}");
                        }
                    }
                    else
                    {
                        //
                    }
                }
                catch (Exception)
                {
                    (sender as FileSystemWatcher).EnableRaisingEvents = false;
                }
            };

            watcher.Changed += (sender, e) =>
            {
                if (_isCreated)
                {
                    _isCreated = false;
                    return;
                }

                if (_isChanged)
                {
                    _isChanged = false;
                    return;
                }

                _isChanged = true;

                try
                {
                    // 更改（除重命名）
                    if (Directory.Exists(e.FullPath))
                    {
                        DirectoryDescriptor oldDir = null;
                        foreach (var val in _directories.Values)
                        {
                            oldDir = val.Key.LookupDirectory(e.FullPath);
                            if (oldDir.IsNotDefault())
                                break;
                        }

                        var parentDir = oldDir?.Parent;
                        if (parentDir.IsNotDefault())
                        {
                            parentDir.Subdirs.Remove(oldDir);
                            parentDir.Size -= oldDir.Size;
                            Logger.LogDebug($"Remove directory: FullName={oldDir.Path},Size={oldDir.FormatSize}");

                            var newInfo = new DirectoryInfo(e.FullPath);
                            var newDir = new DirectoryDescriptor(newInfo, filter);
                            newDir.Parent = parentDir;

                            parentDir.Subdirs.Add(newDir);
                            parentDir.Size += newDir.Size;
                            Logger.LogDebug($"Add new directory: FullName={newDir.Path},Size={newDir.FormatSize}");
                        }
                    }
                    else if (File.Exists(e.FullPath))
                    {
                        FileDescriptor oldFile = null;
                        foreach (var val in _directories.Values)
                        {
                            oldFile = val.Key.LookupFile(e.FullPath);
                            if (oldFile.IsNotDefault())
                                break;
                        }

                        var sameDir = oldFile?.Directory;
                        if (sameDir.IsNotDefault())
                        {
                            sameDir.Subfiles.Remove(oldFile);
                            sameDir.Size -= oldFile.Size;
                            Logger.LogDebug($"Remove file: FullName={oldFile.Path},Size={oldFile.FormatSize}");

                            var newInfo = new FileInfo(e.FullPath);
                            var newFile = new FileDescriptor(newInfo);
                            newFile.Directory = sameDir;

                            sameDir.Subfiles.Add(newFile);
                            sameDir.Size += newFile.Size;
                            Logger.LogDebug($"Add new file: FullName={newFile.Path},Size={newFile.FormatSize}");
                        }
                    }
                    else
                    {
                        //
                    }
                }
                catch (Exception)
                {
                    (sender as FileSystemWatcher).EnableRaisingEvents = false;
                }
            };

            watcher.Renamed += (sender, e) =>
            {
                try
                {
                    if (Directory.Exists(e.FullPath))
                    {
                        DirectoryDescriptor oldDir = null;
                        foreach (var val in _directories.Values)
                        {
                            oldDir = val.Key.LookupDirectory(e.OldFullPath);
                            if (oldDir.IsNotDefault())
                                break;
                        }

                        var parentDir = oldDir?.Parent;
                        if (parentDir.IsNotDefault())
                        {
                            parentDir.Subdirs.Remove(oldDir);
                            parentDir.Size -= oldDir.Size;
                            Logger.LogDebug($"Remove directory: FullName={oldDir.Path},Size={oldDir.FormatSize}");

                            var newInfo = new DirectoryInfo(e.FullPath);
                            var newDir = new DirectoryDescriptor(newInfo, filter);
                            newDir.Parent = parentDir;

                            parentDir.Subdirs.Add(newDir);
                            parentDir.Size += newDir.Size;
                            Logger.LogDebug($"Add new directory: FullName={newDir.Path},Size={newDir.FormatSize}");
                        }
                    }
                    else if (File.Exists(e.FullPath))
                    {
                        FileDescriptor oldFile = null;
                        foreach (var val in _directories.Values)
                        {
                            oldFile = val.Key.LookupFile(e.OldFullPath);
                            if (oldFile.IsNotDefault())
                                break;
                        }

                        var sameDir = oldFile?.Directory;
                        if (sameDir.IsNotDefault())
                        {
                            sameDir.Subfiles.Remove(oldFile);
                            sameDir.Size -= oldFile.Size;
                            Logger.LogDebug($"Remove file: FullName={oldFile.Path},Size={oldFile.FormatSize}");

                            var newInfo = new FileInfo(e.FullPath);
                            var newFile = new FileDescriptor(newInfo);
                            newFile.Directory = sameDir;

                            sameDir.Subfiles.Add(newFile);
                            sameDir.Size += newFile.Size;
                            Logger.LogDebug($"Add file: FullName={newFile.Path},Size={newFile.FormatSize}");
                        }
                    }
                    else
                    {
                        //
                    }
                }
                catch (Exception)
                {
                    (sender as FileSystemWatcher).EnableRaisingEvents = false;
                }
            };
            
            watcher.EnableRaisingEvents = true;

            return watcher;
        }


        /// <summary>
        /// 释放资源。
        /// </summary>
        public virtual void Dispose()
        {
            if (_drivers.IsNotEmpty())
            {
                _drivers.Clear();
                Logger.LogDebug($"Clear all drivers");
            }

            if (_directories.IsNotEmpty())
            {
                foreach (var dir in _directories)
                {
                    // 释放监视器资源
                    dir.Value.Value?.Dispose();
                    Logger.LogDebug($"Remove directory watcher: FullName={dir.Value.Key.Path},Size={dir.Value.Key.FormatSize}");
                }

                _directories.Clear();
                Logger.LogDebug($"Clear all directory watchers");
            }
        }

    }
}
