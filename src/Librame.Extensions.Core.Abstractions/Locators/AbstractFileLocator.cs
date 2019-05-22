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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象文件定位器。
    /// </summary>
    public abstract class AbstractFileLocator : AbstractLocator<string>, IFileLocator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractFileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public AbstractFileLocator(string fileName)
            : base(fileName)
        {
            FileName = Path.GetFileName(fileName);
            BasePath = Path.GetDirectoryName(fileName);
        }


        /// <summary>
        /// 文件名。
        /// </summary>
        public string FileName { get; }
        
        /// <summary>
        /// 基础路径。
        /// </summary>
        public string BasePath { get; private set; }


        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="changeFactory">给定的改变工厂方法。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public IFileLocator ChangeBasePath(Func<string, string> changeFactory)
        {
            var basePath = changeFactory?.Invoke(BasePath);

            return ChangeBasePath(basePath);
        }
        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public IFileLocator ChangeBasePath(string basePath)
        {
            BasePath = basePath.NotNullOrEmpty(nameof(basePath));
            // Update Source
            Source = Path.Combine(BasePath, FileName);

            return this;
        }


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is IFileLocator other)
                return Equals(other);

            return false;
        }
        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(IFileLocator other)
        {
            return Source == other.Source;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }


        /// <summary>
        /// 转换为文件源路径。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Source;
        }

    }
}
