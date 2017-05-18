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
using System.IO;
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// 主机工具。
    /// </summary>
    public class MainframeUtility
    {
        /// <summary>
        /// 获取环境信息。
        /// </summary>
        /// <returns>返回 <see cref="EnvironmentInfo"/>。</returns>
        public static EnvironmentInfo GetEnvironmentInfo()
        {
            return new EnvironmentInfo();
        }

        /// <summary>
        /// 格式化环境信息。
        /// </summary>
        /// <param name="formatFactory">给定的格式化工厂方法。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatEnvironmentInfo(Action<EnvironmentInfo, StringBuilder> formatFactory = null)
        {
            if (ReferenceEquals(formatFactory, null))
            {
                formatFactory = (ei, sb) =>
                {
                    sb.Append(ei.AsPairsString());
                };
            }

            var builder = new StringBuilder();
            formatFactory(GetEnvironmentInfo(), builder);

            return builder.ToString();
        }


        /// <summary>
        /// 获取驱动信息列表。
        /// </summary>
        /// <returns>返回 <see cref="IList{DriveInfoWrapper}"/>。</returns>
        public static IList<DriveInfoWrapper> GetDriveInfos()
        {
            var wrappers = new List<DriveInfoWrapper>();

            var drives = DriveInfo.GetDrives();
            foreach (var d in drives)
            {
                if (d.IsReady)
                {
                    wrappers.Add(new DriveInfoWrapper(d));
                }
            }

            return wrappers;
        }

        /// <summary>
        /// 格式化驱动器信息。
        /// </summary>
        /// <param name="formatFactory">给定的格式化工厂方法。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatDriveInfo(Action<DriveInfoWrapper, int, StringBuilder> formatFactory = null,
            string separator = ";")
        {
            long allDrivesTotalSize;
            return FormatDriveInfo(out allDrivesTotalSize, formatFactory, separator);
        }
        /// <summary>
        /// 格式化驱动器信息。
        /// </summary>
        /// <param name="allDrivesTotalSize">输出所有驱动器总大小。</param>
        /// <param name="formatFactory">给定的格式化工厂方法。</param>
        /// <param name="separator">给定的分隔符。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatDriveInfo(out long allDrivesTotalSize,
            Action<DriveInfoWrapper, int, StringBuilder> formatFactory = null, string separator = ";")
        {
            if (ReferenceEquals(formatFactory, null))
            {
                formatFactory = (di, i, sb) =>
                {
                    sb.AppendFormat("{0}/{1}",
                        di.FormatTotalUsageSpace, di.FormatTotalSize);
                };
            }

            var builder = new StringBuilder();
            var drives = GetDriveInfos();

            allDrivesTotalSize = 0;
            int index = 0;
            foreach (var di in drives)
            {
                allDrivesTotalSize += di.TotalSize;

                formatFactory(di, index, builder);

                if (index != drives.Count - 1)
                    builder.Append(separator);

                index++;
            }

            return builder.ToString();
        }

    }
}
