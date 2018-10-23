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
    /// 驱动器描述符。
    /// </summary>
    public class DriverDescriptor
    {
        private readonly DriveInfo _info;
        private readonly CapacityUnitFormat _format;
        private readonly CapacityUnitNotation _notation;


        /// <summary>
        /// 构造一个 <see cref="DriverDescriptor"/> 实例。
        /// </summary>
        /// <param name="info">给定的 <see cref="DriveInfo"/>。</param>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>（可选；默认使用吉字节单位）。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        public DriverDescriptor(DriveInfo info, CapacityUnitFormat format = CapacityUnitFormat.GByte,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            _info = info.NotDefault(nameof(info));
            _format = format;
            _notation = notation;
        }


        /// <summary>
        /// 卷标。
        /// </summary>
        public string VolumeLabel => _info.VolumeLabel;

        /// <summary>
        /// 空间总大小。
        /// </summary>
        public long TotalSize => _info.TotalSize;

        /// <summary>
        /// 格式化空间总大小。
        /// </summary>
        public string FormatTotalSize => TotalSize.FormatCapacityUnitString(_format, _notation);

        /// <summary>
        /// 可用空闲空间大小。
        /// </summary>
        public long TotalFreeSpace => _info.TotalFreeSpace;

        /// <summary>
        /// 格式化可用空闲空间大小。
        /// </summary>
        public string FormatTotalFreeSpace => TotalFreeSpace.FormatCapacityUnitString(_format, _notation);

        /// <summary>
        /// 已使用空间大小。
        /// </summary>
        public long TotalUsageSpace => _info.TotalSize - _info.TotalFreeSpace;

        /// <summary>
        /// 格式化已使用空间大小。
        /// </summary>
        public string FormatTotalUsageSpace => TotalUsageSpace.FormatCapacityUnitString(_format, _notation);

        /// <summary>
        /// 已使用空间百分比。
        /// </summary>
        public int TotalUsageSpacePercent => Convert.ToInt32((double)TotalUsageSpace / TotalSize * 100);
    }
}
