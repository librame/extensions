#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 容量单位静态扩展。
    /// </summary>
    public static class CapacityUnitExtensions
    {

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（根据文件大小自适应适配容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacityUnitString(this int fileSize,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            var infos = CapacityUnitManager.Infos.Reverse().ToArray();

            for (var i = 0; i < infos.Length; i++)
            {
                var info = infos[i];
                var descriptor = notation == CapacityUnitNotation.Binary ? info.Binary : info.Decimal;

                if (fileSize >= descriptor.Size)
                    return fileSize.FormatCapacityUnitString(descriptor);
            }

            return $"{fileSize} Bytes";
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（指定容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacityUnitString(this int fileSize, CapacityUnitFormat format,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            var descriptor = CapacityUnitManager.GetDescriptor(format, notation);

            return fileSize.FormatCapacityUnitString(descriptor);
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="descriptor">给定的 <see cref="CapacityUnitDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacityUnitString(this int fileSize, CapacityUnitDescriptor descriptor)
        {
            descriptor.NotDefault(nameof(descriptor));

            if (fileSize <= descriptor.Size)
                return $"{fileSize} {descriptor.Abbr}";

            var str = string.Format("{0:0,0.00} " + descriptor.Abbr,
                ((double)fileSize) / descriptor.Size);

            // 移除可能存在的前置0
            return str.TrimStart('0');
        }


        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（根据文件大小自适应适配容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacityUnitString(this long fileSize,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            var infos = CapacityUnitManager.Infos.Reverse().ToArray();

            for (var i = 0; i < infos.Length; i++)
            {
                var info = infos[i];
                var descriptor = notation == CapacityUnitNotation.Binary ? info.Binary : info.Decimal;

                if (fileSize >= descriptor.Size)
                    return fileSize.FormatCapacityUnitString(descriptor);
            }
            
            return $"{fileSize} Bytes";
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（指定容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>（可选；默认使用二进制）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacityUnitString(this long fileSize, CapacityUnitFormat format,
            CapacityUnitNotation notation = CapacityUnitNotation.Binary)
        {
            var descriptor = CapacityUnitManager.GetDescriptor(format, notation);

            return fileSize.FormatCapacityUnitString(descriptor);
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="descriptor">给定的 <see cref="CapacityUnitDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacityUnitString(this long fileSize, CapacityUnitDescriptor descriptor)
        {
            descriptor.NotDefault(nameof(descriptor));

            if (fileSize <= descriptor.Size)
                return $"{fileSize} {descriptor.Abbr}";

            var str = string.Format("{0:0,0.00} " + descriptor.Abbr,
                ((double)fileSize) / descriptor.Size);

            // 移除可能存在的前置0
            return str.TrimStart('0');
        }

    }
}
