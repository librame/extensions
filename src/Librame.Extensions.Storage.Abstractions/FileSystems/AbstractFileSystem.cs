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
using System.IO;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 抽象文件系统。
    /// </summary>
    /// <typeparam name="TFileSystem">指定的文件系统类型。</typeparam>
    public abstract class AbstractFileSystem<TFileSystem> : IFileSystem, IEquatable<TFileSystem>
        where TFileSystem : IFileSystem
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractFileSystem{TFileSystem}"/> 实例。
        /// </summary>
        /// <param name="info">给定的 <see cref="FileSystemInfo"/>。</param>
        public AbstractFileSystem(FileSystemInfo info)
        {
            Info = info.NotDefault(nameof(info));
        }


        /// <summary>
        /// 文件系统信息。
        /// </summary>
        protected FileSystemInfo Info { get; }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name => Info.Name;

        /// <summary>
        /// 路径。
        /// </summary>
        public string Path => Info.FullName;

        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateTime => Info.CreationTime;

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateTime => Info.LastWriteTime;

        /// <summary>
        /// 大小。
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 格式化大小。
        /// </summary>
        public string FormatSize { get; set; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的文件系统类型实例。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(TFileSystem other)
        {
            return Name == other?.Name && Path == other?.Path;
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is TFileSystem other)
                return Equals(other);

            return false;
        }

        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
        {
            return (Name.GetHashCode() ^ Path.GetHashCode());
        }

    }
}
