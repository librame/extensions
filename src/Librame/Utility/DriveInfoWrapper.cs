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

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="DriveInfo"/> 封装。
    /// </summary>
    public class DriveInfoWrapper
    {
        /// <summary>
        /// 获取驱动器信息。
        /// </summary>
        public DriveInfo Info { get; private set; }

        /// <summary>
        /// 获取或设置文件大小单位。
        /// </summary>
        public FileSizeUnit SizeUnit { get; set; }


        /// <summary>
        /// 构造一个 <see cref="DriveInfoWrapper"/> 实例。
        /// </summary>
        /// <param name="info">给定的驱动器信息。</param>
        /// <param name="sizeUnit">给定的文件大小单位。</param>
        public DriveInfoWrapper(DriveInfo info, FileSizeUnit sizeUnit = FileSizeUnit.GiB)
        {
            Info = info.NotNull(nameof(info));
            SizeUnit = sizeUnit;
        }


        /// <summary>
        /// 空间总大小。
        /// </summary>
        public long TotalSize
        {
            get { return Info.TotalSize; }
        }

        /// <summary>
        /// 可用空闲空间大小。
        /// </summary>
        public long TotalFreeSpace
        {
            get { return Info.TotalFreeSpace; }
        }

        /// <summary>
        /// 获取已使用空间大小。
        /// </summary>
        public long TotalUsageSpace
        {
            get { return (TotalSize - TotalFreeSpace); }
        }
        
        /// <summary>
        /// 已使用空间百分比。
        /// </summary>
        public int TotalUsageSpacePercent
        {
            get { return Convert.ToInt32(((double)TotalUsageSpace / TotalSize) * 100); }
        }


        /// <summary>
        /// 格式化空间总大小。
        /// </summary>
        public string FormatTotalSize
        {
            get { return Info.TotalSize.FormatFileSizeUnit(SizeUnit); }
        }

        /// <summary>
        /// 格式化可用空闲空间大小。
        /// </summary>
        public string FormatTotalFreeSpace
        {
            get { return Info.TotalFreeSpace.FormatFileSizeUnit(SizeUnit); }
        }

        /// <summary>
        /// 格式化已使用空间大小。
        /// </summary>
        public string FormatTotalUsageSpace
        {
            get { return TotalUsageSpace.FormatFileSizeUnit(SizeUnit); }
        }

    }
}
