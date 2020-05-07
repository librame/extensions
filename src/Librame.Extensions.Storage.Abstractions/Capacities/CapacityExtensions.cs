#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions
{
    using Storage.Capacities;

    /// <summary>
    /// 容量单位静态扩展。
    /// </summary>
    public static class CapacityExtensions
    {
        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（根据文件大小自适应适配容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="notation">给定的 <see cref="UnitNotation"/>（可选；默认使用 <see cref="UnitNotation.BinarySystem"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacity(this int fileSize,
            UnitNotation notation = UnitNotation.BinarySystem)
        {
            var infos = UnitDefinitionHelper.GetInfos(notation).Reverse();

            foreach (var info in infos)
            {
                if (fileSize >= info.Size)
                    return info.FormatString(fileSize);
            }

            return $"{fileSize} Bytes";
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（指定容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="format">给定的 <see cref="UnitFormat"/>。</param>
        /// <param name="notation">给定的 <see cref="UnitNotation"/>（可选；默认使用 <see cref="UnitNotation.BinarySystem"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacity(this int fileSize,
            UnitFormat format, UnitNotation notation = UnitNotation.BinarySystem)
        {
            var info = UnitDefinitionHelper.GetInfo(format, notation);
            return fileSize.FormatCapacity(info);
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="info">给定的 <see cref="UnitDefinitionInfo"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string FormatCapacity(this int fileSize, UnitDefinitionInfo info)
        {
            info.NotNull(nameof(info));
            return info.FormatString(fileSize);
        }


        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（根据文件大小自适应适配容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="notation">给定的 <see cref="UnitNotation"/>（可选；默认使用 <see cref="UnitNotation.BinarySystem"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacity(this long fileSize,
            UnitNotation notation = UnitNotation.BinarySystem)
        {
            var infos = UnitDefinitionHelper.GetInfos(notation).Reverse();

            foreach (var info in infos)
            {
                if (fileSize >= info.Size)
                    return info.FormatString(fileSize);
            }

            return $"{fileSize} Bytes";
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式（指定容量单位格式）。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="format">给定的 <see cref="UnitFormat"/>。</param>
        /// <param name="notation">给定的 <see cref="UnitNotation"/>（可选；默认使用 <see cref="UnitNotation.BinarySystem"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatCapacity(this long fileSize,
            UnitFormat format, UnitNotation notation = UnitNotation.BinarySystem)
        {
            var info = UnitDefinitionHelper.GetInfo(format, notation);
            return fileSize.FormatCapacity(info);
        }

        /// <summary>
        /// 将文件大小格式化为带容量单位的字符串形式。
        /// </summary>
        /// <param name="fileSize">给定的文件大小。</param>
        /// <param name="info">给定的 <see cref="UnitDefinitionInfo"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string FormatCapacity(this long fileSize, UnitDefinitionInfo info)
        {
            info.NotNull(nameof(info));
            return info.FormatString(fileSize);
        }

    }
}
